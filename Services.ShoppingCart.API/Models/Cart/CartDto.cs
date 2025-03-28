namespace Services.ShoppingCart.API.Models.Cart;

public record CartDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string? CouponCode { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal CartTotal { get; set; }
    public List<CartDetailDto> CartDetails { get; set; }
}