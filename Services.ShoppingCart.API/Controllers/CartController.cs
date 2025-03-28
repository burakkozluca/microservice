using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ShoppingCart.API.Helpers.RabbitMQ;
using Services.ShoppingCart.API.Models.Cart;
using Services.ShoppingCart.API.Models.Cart.Checkout;
using Services.ShoppingCart.API.Services;

namespace Services.ShoppingCart.API.Controllers;

// Controllers/CartController.cs
[ApiController]
[Route("api/[controller]")]
public class CartController : CustomController
{
    private readonly ICartService _cartService;
    private readonly IRabbitMQCartMessageSender _rabbitMQCartMessageSender;

    public CartController(
        ICartService cartService,
        IRabbitMQCartMessageSender rabbitMQCartMessageSender)
    {
        _cartService = cartService;
        _rabbitMQCartMessageSender = rabbitMQCartMessageSender;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(string userId) => 
        CreateActionResult(await _cartService.GetCartAsync(userId));

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request) => 
        CreateActionResult(await _cartService.CreateCartAsync(request));

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateCart([FromBody] UpdateCartRequest request) => 
        CreateActionResult(await _cartService.UpdateCartAsync(request));

    [HttpDelete("{cartId}")]
    [Authorize]
    public async Task<IActionResult> RemoveFromCart(int cartId) => 
        CreateActionResult(await _cartService.RemoveFromCartAsync(cartId));

    [HttpPost("apply-coupon")]
    [Authorize]
    public async Task<IActionResult> ApplyCoupon([FromBody] ApplyCouponRequest request) => 
        CreateActionResult(await _cartService.ApplyCouponAsync(request.UserId, request.CouponCode));

    [HttpPost("checkout")]
    [Authorize]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
    {
        var cart = await _cartService.GetCartAsync(request.UserId);
        if (!cart.IsSuccess)
            return CreateActionResult(cart);

        _rabbitMQCartMessageSender.SendMessage(cart.Data, "checkoutqueue");
        return CreateActionResult(await _cartService.ClearCartAsync(request.UserId));
    }
}