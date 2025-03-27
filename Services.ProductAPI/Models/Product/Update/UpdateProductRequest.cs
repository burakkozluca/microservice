namespace Services.ProductAPI.Models.Product.Update;

public record UpdateProductRequest(int Id, string Name, int Stock, decimal Price,int CategoryId, string Description,IFormFile imageFile);
