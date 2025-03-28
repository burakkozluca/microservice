namespace Services.ShoppingCart.API.Data.Entities;

public class CartDetail
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public Cart Cart { get; set; }
}