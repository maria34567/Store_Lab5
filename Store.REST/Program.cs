using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Common;
using Store.Infrastructure;
using Store.Infrastructure.Models;
using Store.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var dbPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "store_lab3.db"));
builder.Services.AddDbContext<StoreContext>(o => o.UseSqlite($"Data Source={dbPath}"));
builder.Services.AddIdentityApiEndpoints<StoreUser>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<StoreContext>();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IGuidRepository<ElectronicsModel>, GuidRepository<ElectronicsModel>>();
builder.Services.AddScoped<ICrudServiceAsync<Electronics>, ElectronicsCrudService>();
builder.Services.AddScoped<IRepository<CategoryModel>, EfRepository<CategoryModel>>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StoreContext>();
    await db.Database.MigrateAsync();
    if (!await db.Categories.AnyAsync())
    {
        db.Categories.Add(new CategoryModel { Id = 1, Name = "Електроніка" });
        await db.SaveChangesAsync();
    }
    var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    foreach (var role in new[] { "Customer", "Manager", "Admin" })
        if (!await roles.RoleExistsAsync(role))
        {
            var result = await roles.CreateAsync(new IdentityRole(role));
            if (!result.Succeeded) throw new InvalidOperationException($"Cannot create role {role}");
        }
    // One-time local admin bootstrap: supply credentials via environment variables, never commit passwords.
    var email = Environment.GetEnvironmentVariable("STORE_ADMIN_EMAIL");
    var password = Environment.GetEnvironmentVariable("STORE_ADMIN_PASSWORD");
    if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
    {
        var users = scope.ServiceProvider.GetRequiredService<UserManager<StoreUser>>();
        var admin = await users.FindByEmailAsync(email);
        if (admin is null)
        {
            admin = new StoreUser { UserName = email, Email = email, DisplayName = "Administrator", EmailConfirmed = true };
            var result = await users.CreateAsync(admin, password);
            if (!result.Succeeded) throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
            await users.AddToRoleAsync(admin, "Admin");
        }
        // Existing accounts are deliberately not elevated automatically.
    }
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<StoreUser>(); // /login?useCookies=false produces Identity bearer accessToken
app.MapControllers();
app.Run();
