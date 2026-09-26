using Microsoft.AspNetCore.Mvc;
using TicketDesk.BL;
using TicketDesk.DAL;
using TicketDesk.Domain.Entities;

namespace TicketDesk.API.Controllers;

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IGenericRepository<User> _userRepo;
    private readonly PasswordService _passwordService;
    private readonly TokenService _tokenService;

    public AuthController(IGenericRepository<User> userRepo, PasswordService passwordService, TokenService tokenService)
    {
        _userRepo = userRepo;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var users = await _userRepo.GetAllWithIncludesAsync("Role");
        var user = users.FirstOrDefault(u => u.Email == dto.Email);

        if (user is null || !_passwordService.Verify(user, user.PasswordHash, dto.Password))
            return Unauthorized(new { message = "Invalid email or password." });

        var token = _tokenService.GenerateToken(user);
        return Ok(new { token, user.FullName, role = user.Role.Name });
    }
}