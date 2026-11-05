using Microsoft.AspNetCore.Identity;
namespace Store.Infrastructure.Models;
public class StoreUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
}
