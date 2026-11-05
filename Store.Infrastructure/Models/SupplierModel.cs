namespace Store.Infrastructure.Models;
public class SupplierModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ICollection<ProductSupplierModel> ProductSuppliers { get; set; } = new List<ProductSupplierModel>();
}
