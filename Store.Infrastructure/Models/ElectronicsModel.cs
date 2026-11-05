namespace Store.Infrastructure.Models;
public class ElectronicsModel : ProductModel
{
    public string Brand { get; set; } = "";
    public int WarrantyMonths { get; set; }
}
