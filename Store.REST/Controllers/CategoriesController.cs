using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure;
using Store.Infrastructure.Models;
using Store.Infrastructure.Repositories;
using Store.REST.Models;
namespace Store.REST.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController(IRepository<CategoryModel> repository, StoreContext db) : ControllerBase
{
    private static CategoryResponse Map(CategoryModel c) => new(c.Id, c.Name);
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAll() => Ok((await repository.GetAllAsync()).Select(Map));
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponse>> Get(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? NotFound() : Ok(Map(entity));
    }
    [Authorize(Roles = "Customer,Manager,Admin")]
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create(CategoryRequest request)
    {
        var entity = new CategoryModel { Name = request.Name };
        await repository.AddAsync(entity);
        return CreatedAtAction(nameof(Get), new { id = entity.Id }, Map(entity));
    }
    [Authorize(Roles = "Manager,Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CategoryRequest request)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        entity.Name = request.Name;
        await repository.Update(entity);
        return NoContent();
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        if (await db.Products.AnyAsync(p => p.CategoryId == id)) return Conflict("Category contains products.");
        await repository.Delete(entity);
        return NoContent();
    }
}
