using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TicketDesk.BL;
using TicketDesk.BL.Exceptions;
using TicketDesk.DAL;
using TicketDesk.Domain.Entities;
using TicketDesk.Shared;

namespace TicketDesk.API.Controllers;

public class AssignUsersDto
{
    public List<int> UserIds { get; set; } = new();
}

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tickets")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _service;
    private readonly TicketDeskDbContext _db;

    public TicketsController(ITicketService service, TicketDeskDbContext db)
    {
        _service = service;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tickets = await _service.GetAllAsync();
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _service.GetByIdAsync(id);
        return ticket is null ? NotFound() : Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTicketDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Agent,Manager,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    // Who is currently assigned to this ticket (agents and managers)
    [HttpGet("{id}/assignments")]
    public async Task<IActionResult> GetAssignments(int id)
    {
        var exists = await _db.Tickets.AnyAsync(t => t.Id == id);
        if (!exists)
            throw new NotFoundException($"Ticket with id {id} was not found.");

        var agents = await _db.TicketAgents
            .Where(ta => ta.TicketId == id)
            .Select(ta => new { ta.Agent.Id, ta.Agent.FullName })
            .ToListAsync();

        var managers = await _db.TicketManagers
            .Where(tm => tm.TicketId == id)
            .Select(tm => new { tm.Manager.Id, tm.Manager.FullName })
            .ToListAsync();

        return Ok(new { agents, managers });
    }

    // Admin assigns a ticket to one or more Agents
    [HttpPost("{id}/agents")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignAgents(int id, AssignUsersDto dto)
    {
        var ticket = await _db.Tickets.FindAsync(id);
        if (ticket is null)
            throw new NotFoundException($"Ticket with id {id} was not found.");

        var validAgentIds = await _db.Users
            .Where(u => dto.UserIds.Contains(u.Id) && u.Role.Name == "Agent")
            .Select(u => u.Id)
            .ToListAsync();

        var existingLinks = await _db.TicketAgents.Where(ta => ta.TicketId == id).ToListAsync();
        _db.TicketAgents.RemoveRange(existingLinks);

        foreach (var agentId in validAgentIds)
        {
            _db.TicketAgents.Add(new TicketAgent { TicketId = id, AgentId = agentId });
        }

        await _db.SaveChangesAsync();
        return Ok(new { ticketId = id, assignedAgentIds = validAgentIds });
    }

    // Agent shares the ticket with one or more of their OWN Managers only
    [HttpPost("{id}/managers")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> AssignManagers(int id, AssignUsersDto dto)
    {
        var ticket = await _db.Tickets.FindAsync(id);
        if (ticket is null)
            throw new NotFoundException($"Ticket with id {id} was not found.");

        var currentAgentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var validManagerIds = await _db.Users
            .Where(u => dto.UserIds.Contains(u.Id) && u.Role.Name == "Manager" && u.AgentId == currentAgentId)
            .Select(u => u.Id)
            .ToListAsync();

        // Only replace THIS agent's managers, so other agents' managers stay assigned
        var existingLinks = await _db.TicketManagers
            .Where(tm => tm.TicketId == id && tm.Manager.AgentId == currentAgentId)
            .ToListAsync();
        _db.TicketManagers.RemoveRange(existingLinks);

        foreach (var managerId in validManagerIds)
        {
            _db.TicketManagers.Add(new TicketManager { TicketId = id, ManagerId = managerId });
        }

        await _db.SaveChangesAsync();
        return Ok(new { ticketId = id, assignedManagerIds = validManagerIds });
    }

    // Day 3: generic status transition (Open->InProgress, InProgress->Resolved)
    [HttpPost("{id}/status")]
    [Authorize(Roles = "Manager,Agent,Admin")]
    public async Task<IActionResult> ChangeStatus(int id, ChangeStatusDto dto)
    {
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role)!;

        await _service.ChangeStatusAsync(id, dto.Status, currentUserId, currentUserRole);
        return NoContent();
    }

    // Day 4: Agent (checker) finalizes — Resolved -> Closed
    [HttpPost("{id}/close")]
    [Authorize(Roles = "Agent,Admin")]
    public async Task<IActionResult> Close(int id)
    {
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _service.CloseAsync(id, currentUserId);
        return NoContent();
    }

    // Day 4: Agent (checker) sends it back — Resolved -> InProgress
    [HttpPost("{id}/return")]
    [Authorize(Roles = "Agent,Admin")]
    public async Task<IActionResult> Return(int id)
    {
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _service.ReturnAsync(id, currentUserId);
        return NoContent();
    }
}