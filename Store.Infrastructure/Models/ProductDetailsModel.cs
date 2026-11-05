namespace Store.Infrastructure.Models;
public class ProductDetailsModel
{
    public Guid ProductId { get; set; }
    public string Description { get; set; } = "";
    public ProductModel Product { get; set; } = null!;
}
