using Services.ShoppingCart.API.Models.Cart;

namespace Services.ShoppingCart.API.Services;

public interface ICartService
{
    Task<ServiceResult<CartDto>> GetCartAsync(string userId);
    Task<ServiceResult<CartDto>> CreateCartAsync(CreateCartRequest request);
    Task<ServiceResult<CartDto>> UpdateCartAsync(UpdateCartRequest request);
    Task<ServiceResult> RemoveFromCartAsync(int cartId);
    Task<ServiceResult> ApplyCouponAsync(string userId, string couponCode);
    Task<ServiceResult> RemoveCouponAsync(string userId);
    Task<ServiceResult> ClearCartAsync(string userId);
} 