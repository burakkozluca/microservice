namespace Services.ProductAPI.Models.Product;

public record ProductDto(int id , string name , decimal price ,  int stock,int categoryId , string description , string imageUrl , string categoryName);