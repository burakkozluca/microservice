namespace Services.ShoppingCart.API.Models.Cart.Checkout;

public record ApplyCouponRequest
{
    public string UserId { get; set; }
    public string CouponCode { get; set; }
} 