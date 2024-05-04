using Microsoft.EntityFrameworkCore;
using Beedle.Entities;
using Beedle.DbContexts;
using Beedle.Api.Utilities;


var builder = WebApplication.CreateBuilder(args);

// setup SQL database
var connectionString = builder.Configuration.GetConnectionString("Items") ?? "Data Source=Items.db";
builder.Services.AddSqlite<ItemDb>(connectionString);
var userConnectionString = builder.Configuration.GetConnectionString("Users") ?? "Data Source=Users.db";
builder.Services.AddSqlite<UserDb>(userConnectionString);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddHttpClient("FlaskClient", client =>
{
    client.BaseAddress = new Uri("http://127.0.0.1:5000/"); // Replace with the actual URL of your Flask server
});



// allow any origin...not great but gets rid of cors errors for time being
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// this is part of the swagger page, don't delete
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});

var app = builder.Build();
app.UseCors("AllowAll");


if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

var Items = app.MapGroup("/items");
var Users = app.MapGroup("/users");


// ITEM DATABASE FUNCTIONS
Items.MapGet("/", async(ItemDb db) =>
    await db.Items.ToListAsync());


// Update item by ID
Items.MapPut("/{id}", async (int id, Item inputItem, ItemDb db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    item.Name = inputItem.Name;
    item.Quantity = inputItem.Quantity;
    item.Price = inputItem.Price;
    // if no class specified, auto compute
    if (inputItem.Class == null && item.Class == null){
      var result = await FlaskApiUtil.ClassifyItemAsync(inputItem.Name);
      item.Class = result.Class;
    }

    await db.SaveChangesAsync();
    return Results.NoContent();
});


Items.MapGet("/{id}", async (int id, ItemDb db) =>
  await db.Items.FindAsync(id)
    is Item item
            ? Results.Ok(item)
            : Results.NotFound());

Items.MapGet("/category/{category}", async (string category, ItemDb db) => 
{
  List<Item> allItems =  await db.Items.ToListAsync();
  List<Item> returnItems = [];
  foreach (var item in allItems)
  {
    if (item.Class == category){
      returnItems.Add(item);
    }
  }
  return returnItems;
});


Items.MapPost("/", async (Item item, ItemDb db) => {
  if (item.Class == null){ // if no class specified, determine
    var result = await FlaskApiUtil.ClassifyItemAsync(item.Name);
    item.Class = result.Class;
  }
  db.Items.Add(item);
  await db.SaveChangesAsync();

  return Results.Created($"/items/{item.Id}", item);
});


Items.MapDelete("/{id}", async (int id, ItemDb db) =>
{
    Console.WriteLine("In function");
    if (await db.Items.FindAsync(id) is Item item)
    {
        db.Items.Remove(item);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

// USER FUNCTIONS
Users.MapPost("/", async (User user, UserDb db) =>
{
  db.Users.Add(user);
  await db.SaveChangesAsync();

  return Results.Created($"/users/{user.Id}", user);
});
Users.MapGet("/", async(UserDb db) =>
{
  return await db.Users.ToListAsync();
});

Users.MapPost("/login", async(string name, UserDb db) => 
{
     // Find the user by name
    var user = await db.Users.FirstOrDefaultAsync(u => u.Name == name);
    Console.WriteLine("IN functun");
    // Check if a user with the given name exists
    if (user is null)
    {
        // Create a new user with a default balance
        user = new User
        {
            Name = name,
            Balance = 100 // Set the initial balance
        };

        // Add the new user to the database
        db.Users.Add(user);
        await db.SaveChangesAsync();
    }

    // Return the found or newly created user
    return Results.Ok(user);
});

// when the chris is bought
app.MapPost("/purchase", async(PurchaseRequest request, UserDb userDb, ItemDb itemDb)=>{
  using (var transaction = await itemDb.Database.BeginTransactionAsync()){
  var item = await itemDb.Items.FindAsync(request.ItemId);
  if (item is null || item.Quantity < request.Quantity)
  {
    var errorResponse = new {
      status="error",
      message= "We either don't have the item in stock, or not enough.",
      error_code = "quantity_exceeded"
    };
    return Results.NotFound(errorResponse);
  }
  var user = await userDb.Users.FindAsync(request.UserId);
  if (user is null)
  {
    var errorResponse = new {
      status = "error",
      message="User does not exist in our database.",
      error_code = "user_not_found"
    };
    return Results.NotFound(errorResponse);
  }
  // ensure the user can afford the item
  if (user.Balance < item.Price * request.Quantity)
  {
    var errorResponse = new {
        status = "error",
        message = "You do not have enough rupees!",
        error_code = "insufficient_balance"
    };
    return Results.BadRequest(errorResponse);
  }
  if (item.Quantity < request.Quantity){
     var errorResponse = new {
        status = "error",
        message = "The quantity requested exceeds the available stock.",
        error_code = "quantity_exceeded"
    };
    return Results.BadRequest(errorResponse);
  }
  // if everything is okay, update the values
  user.Balance -= request.Quantity * item.Price;
  item.Quantity -= request.Quantity;

  await userDb.SaveChangesAsync();
  await itemDb.SaveChangesAsync();
  await transaction.CommitAsync();
  Console.WriteLine("Purchased");
  var results = new {
    message = "Purchase succesful!",
    ok = true,
  };
  return Results.Ok(results);
}});

app.MapPost("/callFlaskClassify", async (IHttpClientFactory clientFactory) =>
{
    var client = clientFactory.CreateClient("FlaskClient");
    var requestBody = new { word = "example" };
    var flaskResponse = await client.PostAsJsonAsync("classify", requestBody);
    if (!flaskResponse.IsSuccessStatusCode)
        return Results.Problem("Failed to connect to the Flask server.");

    var responseContent = await flaskResponse.Content.ReadFromJsonAsync<dynamic>();
    return Results.Ok(responseContent);
});
// 
app.MapPost("/updateDatabaseClasses", async (ItemDb db) =>
{
    var items = await db.Items.ToListAsync();
    foreach (var item in items)
    {
      try {
      // custom class to abstract this repeated call
      var result = await FlaskApiUtil.ClassifyItemAsync(item.Name);
      item.Class = result.Class;
      }
      catch(Exception ex)
        {
            Console.WriteLine($"Error processing item {item.Name}: {ex.Message}");
            continue;
        }
        
    }

    await db.SaveChangesAsync();
    return Results.Ok("Categories updated successfully");
});


app.MapPost("/test", async(Item item) => {
  var result = await FlaskApiUtil.ClassifyItemAsync(item.Name);
  return result;
});



app.Run();