using System.ComponentModel.DataAnnotations;
namespace Store.REST.Models;
public record RegisterRequest([Required, EmailAddress] string Email, [Required, MinLength(6)] string Password, string? DisplayName);
public record RoleAssignmentRequest([Required, EmailAddress] string Email, [Required] string Role);
