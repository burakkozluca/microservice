using Services.ShoppingCart.API.Models.Cart.Create;

namespace Services.ShoppingCart.API.Models.Cart;

public record CreateCartRequest
{
    public string UserId { get; set; }
    public List<CartDetailRequest> CartDetails { get; set; }
}