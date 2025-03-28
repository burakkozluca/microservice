namespace Services.ShoppingCart.API.Models.Cart;

public record CartDetailDto
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    public string? ImageUrl { get; set; }
} 