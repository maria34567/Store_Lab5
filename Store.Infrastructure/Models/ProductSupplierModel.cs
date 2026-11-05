namespace Store.Infrastructure.Models;
public class ProductSupplierModel
{
    public Guid ProductId { get; set; }
    public ProductModel Product { get; set; } = null!;
    public int SupplierId { get; set; }
    public SupplierModel Supplier { get; set; } = null!;
}
