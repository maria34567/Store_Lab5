using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Store.Infrastructure;
public class StoreContextFactory : IDesignTimeDbContextFactory<StoreContext>
{
    public StoreContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<StoreContext>().UseSqlite("Data Source=store_lab3.db").Options;
        return new StoreContext(options);
    }
}
