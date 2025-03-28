using Services.ShoppingCart.API.Models.Cart.Create;

namespace Services.ShoppingCart.API.Models.Cart;

public record UpdateCartRequest
{
    public int Id { get; set; }
    public string? CouponCode { get; set; }
    public List<CartDetailRequest> CartDetails { get; set; }
}