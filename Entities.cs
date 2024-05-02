namespace Beedle.Entities{
public class Item{
  public int Id { get; set; }
  public string? Name { get; set; }
  public int Quantity { get; set; }
  public int Price { get; set; }
}
public class User
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public int Balance { get; set; }
}
public class PurchaseRequest
{
  // app.MapPost("/purchase", async(int userId, int itemId, 
  // int quantity, UserDb userDb, ItemDb itemDb)=>{
  public int UserId {get; set;}
  public int ItemId { get; set;}
  public int Quantity { get; set; }
}
}