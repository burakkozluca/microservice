using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.ShoppingCart.API.Data;
using Services.ShoppingCart.API.Data.Entities;
using Services.ShoppingCart.API.Models.Cart;
using Services.ShoppingCart.API.Models.Coupon;
using System.Net;

namespace Services.ShoppingCart.API.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public CartService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ServiceResult<CartDto>> GetCartAsync(string userId)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.CartDetails)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            return ServiceResult<CartDto>.Fail("Cart not found", HttpStatusCode.NotFound);
        }

        var cartDto = _mapper.Map<CartDto>(cart);
        return ServiceResult<CartDto>.Success(cartDto);
    }

    public async Task<ServiceResult<CartDto>> CreateCartAsync(CreateCartRequest request)
    {
        var existingCart = await _dbContext.Carts
            .Include(c => c.CartDetails)
            .FirstOrDefaultAsync(c => c.UserId == request.UserId);

        if (existingCart != null)
        {
            return ServiceResult<CartDto>.Fail("Cart already exists for this user", HttpStatusCode.BadRequest);
        }

        var cart = _mapper.Map<Cart>(request);
        await _dbContext.Carts.AddAsync(cart);
        await _dbContext.SaveChangesAsync();

        var cartDto = _mapper.Map<CartDto>(cart);
        return ServiceResult<CartDto>.SuccessCreated(cartDto, $"api/cart/{cart.Id}");
    }

    public async Task<ServiceResult<CartDto>> UpdateCartAsync(UpdateCartRequest request)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.CartDetails)
            .FirstOrDefaultAsync(c => c.Id == request.Id);

        if (cart == null)
        {
            return ServiceResult<CartDto>.Fail("Cart not found", HttpStatusCode.NotFound);
        }

        cart = _mapper.Map(request, cart);
        _dbContext.Carts.Update(cart);
        await _dbContext.SaveChangesAsync();

        var cartDto = _mapper.Map<CartDto>(cart);
        return ServiceResult<CartDto>.Success(cartDto);
    }

    public async Task<ServiceResult> RemoveFromCartAsync(int cartId)
    {
        var cart = await _dbContext.Carts.FindAsync(cartId);
        if (cart == null)
        {
            return ServiceResult.Fail("Cart not found", HttpStatusCode.NotFound);
        }

        _dbContext.Carts.Remove(cart);
        await _dbContext.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult> ApplyCouponAsync(string userId, string couponCode)
    {
        var cart = await _dbContext.Carts
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            return ServiceResult.Fail("Cart not found", HttpStatusCode.NotFound);
        }

        var coupon = await _dbContext.Coupons
            .FirstOrDefaultAsync(c => c.Code == couponCode && c.IsActive);

        if (coupon == null)
        {
            return ServiceResult.Fail("Invalid or expired coupon", HttpStatusCode.BadRequest);
        }

        if (cart.CartTotal < coupon.MinAmount)
        {
            return ServiceResult.Fail($"Minimum amount of {coupon.MinAmount} is required to apply this coupon", 
                HttpStatusCode.BadRequest);
        }

        cart.CouponCode = couponCode;
        cart.DiscountAmount = coupon.DiscountAmount;
        _dbContext.Carts.Update(cart);
        await _dbContext.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult> RemoveCouponAsync(string userId)
    {
        var cart = await _dbContext.Carts
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            return ServiceResult.Fail("Cart not found", HttpStatusCode.NotFound);
        }

        cart.CouponCode = null;
        cart.DiscountAmount = 0;
        _dbContext.Carts.Update(cart);
        await _dbContext.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult> ClearCartAsync(string userId)
    {
        var cart = await _dbContext.Carts
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            return ServiceResult.Fail("Cart not found", HttpStatusCode.NotFound);
        }

        _dbContext.Carts.Remove(cart);
        await _dbContext.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}