namespace Services.ShoppingCart.API.Models.Coupon;

public record CouponDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public decimal MinAmount { get; set; }
}