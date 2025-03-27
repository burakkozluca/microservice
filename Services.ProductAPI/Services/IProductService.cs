using Services.ProductAPI.Models.Product;
using Services.ProductAPI.Models.Product.Create;
using Services.ProductAPI.Models.Product.Update;

namespace Services.ProductAPI.Services;

public interface IProductService
{
    Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request);
    Task<ServiceResult> DeleteAsync(int id);
    Task<ServiceResult<List<ProductDto>>> GetAllListAsync();
    Task<ServiceResult<ProductDto?>> GetByIdAsync(int id);
    Task<ServiceResult> UpdateAsync(UpdateProductRequest request);
    Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request);
}