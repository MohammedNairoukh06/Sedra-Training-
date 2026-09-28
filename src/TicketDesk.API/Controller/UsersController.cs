using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketDesk.BL;
using TicketDesk.DAL;
using TicketDesk.Domain.Entities;
using TicketDesk.Shared;

namespace TicketDesk.API.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Role> _roleRepo;
    private readonly PasswordService _passwordService;

    public UsersController(IGenericRepository<User> userRepo, IGenericRepository<Role> roleRepo, PasswordService passwordService)
    {
        _userRepo = userRepo;
        _roleRepo = roleRepo;
        _passwordService = passwordService;
    }

    // Admin creates Agents
    [HttpPost("agents")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAgent(CreateUserDto dto)
    {
        var agentRole = (await _roleRepo.GetAllAsync()).FirstOrDefault(r => r.Name == "Agent");
        if (agentRole is null) return BadRequest("Agent role not found.");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            RoleId = agentRole.Id
        };
        user.PasswordHash = _passwordService.Hash(user, dto.Password);

        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();

        return Ok(new UserDto { Id = user.Id, FullName = user.FullName, Email = user.Email, Role = agentRole.Name });
    }

    // Admin lists all Agents (used by the assign-agents UI)
    [HttpGet("agents")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAgents()
    {
        var allUsers = await _userRepo.GetAllWithIncludesAsync("Role");

        var agents = allUsers
            .Where(u => u.Role.Name == "Agent")
            .Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role.Name
            });

        return Ok(agents);
    }

    // Agent creates a Manager, auto-linked to themselves
    [HttpPost("managers")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> CreateManager(CreateUserDto dto)
    {
        var currentAgentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var managerRole = (await _roleRepo.GetAllAsync()).FirstOrDefault(r => r.Name == "Manager");
        if (managerRole is null) return BadRequest("Manager role not found.");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            RoleId = managerRole.Id,
            AgentId = currentAgentId
        };
        user.PasswordHash = _passwordService.Hash(user, dto.Password);

        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();

        return Ok(new UserDto { Id = user.Id, FullName = user.FullName, Email = user.Email, Role = managerRole.Name, AgentId = currentAgentId });
    }

    // List the current Agent's own Managers
    [HttpGet("my-managers")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> GetMyManagers()
    {
        var currentAgentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var allUsers = await _userRepo.GetAllWithIncludesAsync("Role");
        var myManagers = allUsers.Where(u => u.AgentId == currentAgentId);

        var result = myManagers.Select(m => new UserDto
        {
            Id = m.Id,
            FullName = m.FullName,
            Email = m.Email,
            Role = m.Role.Name,
            AgentId = m.AgentId
        });

        return Ok(result);
    }
}