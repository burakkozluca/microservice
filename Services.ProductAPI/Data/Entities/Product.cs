namespace Services.ProductAPI.Data.Entities;

public class Product :IAuditEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}