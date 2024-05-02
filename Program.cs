using Microsoft.EntityFrameworkCore;
using Beedle.Entities;
using Beedle.DbContexts;


var builder = WebApplication.CreateBuilder(args);

// setup SQL database
var connectionString = builder.Configuration.GetConnectionString("Items") ?? "Data Source=Items.db";
builder.Services.AddSqlite<ItemDb>(connectionString);
var userConnectionString = builder.Configuration.GetConnectionString("Users") ?? "Data Source=Users.db";
builder.Services.AddSqlite<UserDb>(userConnectionString);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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

    await db.SaveChangesAsync();

    return Results.NoContent();
});


Items.MapGet("/{id}", async (int id, ItemDb db) =>
  await db.Items.FindAsync(id)
    is Item item
            ? Results.Ok(item)
            : Results.NotFound());


Items.MapPost("/", async (Item item, ItemDb db) => {
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
    return Results.NotFound("We either don't have the item in stock, or not enough.");
  }
  var user = await userDb.Users.FindAsync(request.UserId);
  if (user is null)
  {
    return Results.NotFound("User is not in our records.");
  }
  // ensure the user can afford the item
  if (user.Balance < item.Price * request.Quantity)
  {
    return Results.Problem("User cannot afford.");
  }
  // if everything is okay, update the values
  user.Balance -= request.Quantity * item.Price;
  item.Quantity -= request.Quantity;

  await userDb.SaveChangesAsync();
  await itemDb.SaveChangesAsync();
  await transaction.CommitAsync();
  Console.WriteLine("Purchased");
  return Results.Ok("Purchase succesful!");
}});
app.Run();