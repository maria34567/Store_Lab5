using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Store.Infrastructure.Models;

namespace Store.Infrastructure;

public class StoreContext(DbContextOptions<StoreContext> options) : IdentityDbContext<StoreUser>(options)
{
    public DbSet<ProductModel> Products => Set<ProductModel>();
    public DbSet<ElectronicsModel> Electronics => Set<ElectronicsModel>();
    public DbSet<CategoryModel> Categories => Set<CategoryModel>();
    public DbSet<ProductDetailsModel> ProductDetails => Set<ProductDetailsModel>();
    public DbSet<SupplierModel> Suppliers => Set<SupplierModel>();
    public DbSet<ProductSupplierModel> ProductSuppliers => Set<ProductSupplierModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<StoreUser>().Property(u => u.DisplayName).HasMaxLength(100);
        modelBuilder.Entity<ProductModel>().UseTptMappingStrategy();
        modelBuilder.Entity<ProductModel>().ToTable("Products").HasKey(x => x.Id);
        modelBuilder.Entity<ElectronicsModel>().ToTable("Electronics");
        modelBuilder.Entity<ProductModel>().Property(x => x.Name).IsRequired().HasMaxLength(150);
        modelBuilder.Entity<ProductModel>().Property(x => x.Price).HasConversion<double>();
        modelBuilder.Entity<CategoryModel>().ToTable("Categories").HasKey(x => x.Id);
        modelBuilder.Entity<CategoryModel>().Property(x => x.Name).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<ProductModel>().HasOne(x => x.Category).WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ProductDetailsModel>().ToTable("ProductDetails").HasKey(x => x.ProductId);
        modelBuilder.Entity<ProductModel>().HasOne(x => x.Details).WithOne(x => x.Product)
            .HasForeignKey<ProductDetailsModel>(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<SupplierModel>().ToTable("Suppliers").HasKey(x => x.Id);
        modelBuilder.Entity<SupplierModel>().Property(x => x.Name).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<ProductSupplierModel>().ToTable("ProductSuppliers")
            .HasKey(x => new { x.ProductId, x.SupplierId });
        modelBuilder.Entity<ProductSupplierModel>().HasOne(x => x.Product)
            .WithMany(x => x.ProductSuppliers).HasForeignKey(x => x.ProductId);
        modelBuilder.Entity<ProductSupplierModel>().HasOne(x => x.Supplier)
            .WithMany(x => x.ProductSuppliers).HasForeignKey(x => x.SupplierId);
    }
}
