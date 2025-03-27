namespace Services.ProductAPI.Models.Product.Create;

public record CreateProductRequest(int Stock , string Name, decimal Price, int CategoryId, string Description , IFormFile imageFile);

