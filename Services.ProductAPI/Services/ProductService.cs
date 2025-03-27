using Microsoft.EntityFrameworkCore;
using Services.ProductAPI.Data;
using Services.ProductAPI.Models;
using Services.ProductAPI.Models.Product.Create;
using Services.ProductAPI.Models.Product.Update;
using System.Net;
using AutoMapper;
using Services.ProductAPI.Data.Entities;
using Services.ProductAPI.Helpers;
using Services.ProductAPI.Models.Product;

namespace Services.ProductAPI.Services;

public class ProductService
        (
    AppDbContext dbContext,
        IMapper mapper,
        IFileService fileService
        ) : IProductService
    {
        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
        var anyProduct = await dbContext.Products.AnyAsync(x => x.Name == request.Name);

            if (anyProduct)
            {
                return ServiceResult<CreateProductResponse>.Fail("ürün ismi veritabanında bulunmaktadır.",
                    HttpStatusCode.BadRequest);
            }
            string? imageUrl = null;

            if (request.imageFile is not null)
            {
                imageUrl = await fileService.SaveFileAsync(request.imageFile);
            }

            var product = mapper.Map<Product>(request);
            product.ImageUrl = imageUrl;

        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();
            return ServiceResult<CreateProductResponse>.SuccessCreated(new CreateProductResponse(product.Id)
                ,$"api/products/{product.Id}"); 
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
        var product = await dbContext.Products.FindAsync(id);

            if (product == null) { 
                return ServiceResult.Fail("Product not found" , HttpStatusCode.NotFound);
            }

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
        var products = await dbContext.Products.ToListAsync();

            var productsDtos = mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productsDtos);
        }

        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
        var product = await dbContext.Products.FindAsync(id);

            if (product is null)
            {
                return ServiceResult<ProductDto?>.Fail("Product not found", HttpStatusCode.NotFound);
            }

            var productDto = mapper.Map<ProductDto>(product);

            return ServiceResult<ProductDto?>.Success(productDto, HttpStatusCode.OK)!;
        }
        
        public async Task<ServiceResult> UpdateAsync(UpdateProductRequest request)
        {
        var product = await dbContext.Products.FindAsync(request.Id);
 
            if (product is null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }
 
            string? productImg = product.ImageUrl;
 
 
        var isProductNameExist = await dbContext.Products.AnyAsync(x => x.Name == request.Name && x.Id != product.Id);
 
            if (isProductNameExist)
            {
                return ServiceResult.Fail("ürün ismi veritabanında bulunmaktadır.",
                    HttpStatusCode.BadRequest);
            }
 
            string? imageUrl = null;
 
            if (request.imageFile is not null)
            {
                imageUrl = await fileService.SaveFileAsync(request.imageFile);
                product.ImageUrl = imageUrl;
            }
            else
            {
                product.ImageUrl = productImg;
            }

            product = mapper.Map(request, product);
 
        dbContext.Products.Update(product);
 
        await dbContext.SaveChangesAsync();
 
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
 
        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
        var product = await dbContext.Products.FindAsync(request.ProductId);

            if (product is null)
            {
                return ServiceResult.Fail("Product not found" , HttpStatusCode.NotFound);
            }

            product.Stock = request.Stock;

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
    }
    }