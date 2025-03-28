namespace Services.ShoppingCart.API.Data.Entities;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string? CouponCode { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal CartTotal { get; set; }
    public List<CartDetail> CartDetails { get; set; }
}