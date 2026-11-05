using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Common;
using Store.REST.Models;
namespace Store.REST.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(ICrudServiceAsync<Electronics> service) : ControllerBase
{
    private static ProductResponse Map(Electronics x) => new(x.Id, x.Name, x.Price, x.Brand, x.WarrantyMonths);
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll([FromQuery] int? page, [FromQuery] int? amount)
    {
        if (page.HasValue != amount.HasValue || (page.HasValue && (page < 1 || amount < 1))) return BadRequest("Use positive page and amount together.");
        var items = page.HasValue ? await service.ReadAllAsync(page.Value, amount!.Value) : await service.ReadAllAsync();
        return Ok(items.Select(Map));
    }
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> Get(Guid id)
    {
        try { return Ok(Map(await service.ReadAsync(id))); }
        catch (KeyNotFoundException) { return NotFound(); }
    }
    [Authorize(Roles = "Customer,Manager,Admin")]
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(ProductCreateRequest request)
    {
        var entity = new Electronics(request.Name, request.Price, request.Brand, request.WarrantyMonths);
        if (!await service.CreateAsync(entity)) return Conflict();
        await service.SaveAsync();
        return CreatedAtAction(nameof(Get), new { id = entity.Id }, Map(entity));
    }
    [Authorize(Roles = "Manager,Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, ProductUpdateRequest request)
    {
        var entity = new Electronics(request.Name, request.Price, request.Brand, request.WarrantyMonths) { Id = id };
        if (!await service.UpdateAsync(entity)) return NotFound();
        await service.SaveAsync();
        return NoContent();
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        Electronics entity;
        try { entity = await service.ReadAsync(id); }
        catch (KeyNotFoundException) { return NotFound(); }
        if (!await service.RemoveAsync(entity)) return NotFound();
        await service.SaveAsync();
        return NoContent();
    }
}
