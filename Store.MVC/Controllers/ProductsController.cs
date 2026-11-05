using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure;
namespace Store.MVC.Controllers;
public class ProductsController(StoreContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Electronics.AsNoTracking().OrderBy(x => x.Name).Take(100).ToListAsync());
    public async Task<IActionResult> Details(Guid id)
    {
        var item = await db.Electronics.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : View(item);
    }
}
