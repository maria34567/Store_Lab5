namespace Store.Infrastructure.Models;

public abstract class ProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public CategoryModel Category { get; set; } = null!;
    public ProductDetailsModel? Details { get; set; }
    public ICollection<ProductSupplierModel> ProductSuppliers { get; set; } = new List<ProductSupplierModel>();
}
