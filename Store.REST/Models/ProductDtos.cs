using System.ComponentModel.DataAnnotations;
namespace Store.REST.Models;
public sealed record ProductResponse(Guid Id, string Name, decimal Price, string Brand, int WarrantyMonths);
public sealed record ProductCreateRequest([Required, StringLength(150)] string Name, [Range(0.01, 100000000)] decimal Price, [Required] string Brand, [Range(0, 120)] int WarrantyMonths);
public sealed record ProductUpdateRequest([Required, StringLength(150)] string Name, [Range(0.01, 100000000)] decimal Price, [Required] string Brand, [Range(0, 120)] int WarrantyMonths);
public sealed record CategoryResponse(int Id, string Name);
public sealed record CategoryRequest([Required, StringLength(100)] string Name);
