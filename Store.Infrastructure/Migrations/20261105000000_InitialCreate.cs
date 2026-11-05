using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Store.Infrastructure;

#nullable disable
namespace Store.Infrastructure.Migrations;

[DbContext(typeof(StoreContext))]
[Migration("20261105000000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(name:"Categories",columns:table=>new
        {
            Id=table.Column<int>(type:"INTEGER",nullable:false).Annotation("Sqlite:Autoincrement",true),
            Name=table.Column<string>(type:"TEXT",maxLength:100,nullable:false)
        },constraints:table=>table.PrimaryKey("PK_Categories",x=>x.Id));
        migrationBuilder.CreateTable(name:"Suppliers",columns:table=>new
        {
            Id=table.Column<int>(type:"INTEGER",nullable:false).Annotation("Sqlite:Autoincrement",true),
            Name=table.Column<string>(type:"TEXT",maxLength:100,nullable:false)
        },constraints:table=>table.PrimaryKey("PK_Suppliers",x=>x.Id));
        migrationBuilder.CreateTable(name:"Products",columns:table=>new
        {
            Id=table.Column<Guid>(type:"TEXT",nullable:false),
            Name=table.Column<string>(type:"TEXT",maxLength:150,nullable:false),
            Price=table.Column<double>(type:"REAL",nullable:false),
            CategoryId=table.Column<int>(type:"INTEGER",nullable:false)
        },constraints:table=>{table.PrimaryKey("PK_Products",x=>x.Id);table.ForeignKey("FK_Products_Categories_CategoryId",x=>x.CategoryId,"Categories","Id",onDelete:ReferentialAction.Restrict);});
        migrationBuilder.CreateTable(name:"Electronics",columns:table=>new
        {
            Id=table.Column<Guid>(type:"TEXT",nullable:false),
            Brand=table.Column<string>(type:"TEXT",nullable:false),
            WarrantyMonths=table.Column<int>(type:"INTEGER",nullable:false)
        },constraints:table=>{table.PrimaryKey("PK_Electronics",x=>x.Id);table.ForeignKey("FK_Electronics_Products_Id",x=>x.Id,"Products","Id",onDelete:ReferentialAction.Cascade);});
        migrationBuilder.CreateTable(name:"ProductDetails",columns:table=>new
        {
            ProductId=table.Column<Guid>(type:"TEXT",nullable:false),
            Description=table.Column<string>(type:"TEXT",nullable:false)
        },constraints:table=>{table.PrimaryKey("PK_ProductDetails",x=>x.ProductId);table.ForeignKey("FK_ProductDetails_Products_ProductId",x=>x.ProductId,"Products","Id",onDelete:ReferentialAction.Cascade);});
        migrationBuilder.CreateTable(name:"ProductSuppliers",columns:table=>new
        {
            ProductId=table.Column<Guid>(type:"TEXT",nullable:false),
            SupplierId=table.Column<int>(type:"INTEGER",nullable:false)
        },constraints:table=>{table.PrimaryKey("PK_ProductSuppliers",x=>new{x.ProductId,x.SupplierId});table.ForeignKey("FK_ProductSuppliers_Products_ProductId",x=>x.ProductId,"Products","Id",onDelete:ReferentialAction.Cascade);table.ForeignKey("FK_ProductSuppliers_Suppliers_SupplierId",x=>x.SupplierId,"Suppliers","Id",onDelete:ReferentialAction.Cascade);});
        migrationBuilder.CreateIndex(name:"IX_Products_CategoryId",table:"Products",column:"CategoryId");
        migrationBuilder.CreateIndex(name:"IX_ProductSuppliers_SupplierId",table:"ProductSuppliers",column:"SupplierId");
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("Electronics");migrationBuilder.DropTable("ProductDetails");migrationBuilder.DropTable("ProductSuppliers");migrationBuilder.DropTable("Products");migrationBuilder.DropTable("Suppliers");migrationBuilder.DropTable("Categories");
    }
}
