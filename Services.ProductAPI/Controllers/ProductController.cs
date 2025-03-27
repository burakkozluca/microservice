using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ProductAPI.Models.Product.Create;
using Services.ProductAPI.Models.Product.Update;
using Services.ProductAPI.Services;

namespace Services.ProductAPI.Controllers;

[Authorize]
    public class ProductController(IProductService productService) : CustomController
    {
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetAll() => CreateActionResult(await productService.GetAllListAsync());

  

        [HttpGet("{id:int}"), AllowAnonymous]
        public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetByIdAsync(id));


       
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request) => CreateActionResult(await productService.CreateAsync(request));
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateProductRequest request) => CreateActionResult(await productService.UpdateAsync(request));
        
        [HttpPatch("stock")]
        public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request) => CreateActionResult(await productService.UpdateStockAsync(request));


        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Delete(int id) => CreateActionResult(await productService.DeleteAsync(id));


    }
