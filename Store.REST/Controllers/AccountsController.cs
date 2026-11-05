using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Infrastructure.Models;
using Store.REST.Models;

namespace Store.REST.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController(UserManager<StoreUser> users) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = new StoreUser { UserName = request.Email, Email = request.Email, DisplayName = request.DisplayName ?? "" };
        var result = await users.CreateAsync(user, request.Password);
        if (!result.Succeeded) return BadRequest(result.Errors.Select(e => e.Description));
        var roleResult = await users.AddToRoleAsync(user, "Customer");
        if (!roleResult.Succeeded) return StatusCode(500, "Failed to assign default role.");
        return Created($"/api/accounts/{user.Id}", new { user.Id, user.Email, Role = "Customer" });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var user = await users.GetUserAsync(User);
        if (user is null) return Unauthorized();
        return Ok(new { user.Id, user.Email, user.DisplayName, Roles = await users.GetRolesAsync(user) });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole(RoleAssignmentRequest request)
    {
        if (request.Role is not ("Customer" or "Manager" or "Admin")) return BadRequest("Invalid role.");
        var user = await users.FindByEmailAsync(request.Email);
        if (user is null) return NotFound();
        var result = await users.AddToRoleAsync(user, request.Role);
        return result.Succeeded ? NoContent() : BadRequest(result.Errors.Select(e => e.Description));
    }
}
