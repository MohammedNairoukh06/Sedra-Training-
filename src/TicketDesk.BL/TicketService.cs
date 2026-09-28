using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketDesk.BL.Exceptions;
using TicketDesk.DAL;
using TicketDesk.Domain.Entities;
using TicketDesk.Domain.Enums;
using TicketDesk.Shared;

namespace TicketDesk.BL;

public class TicketService : ITicketService
{
    private readonly IGenericRepository<Ticket> _repo;
    private readonly IGenericRepository<Category> _categoryRepo;
    private readonly TicketDeskDbContext _db;
    private readonly IMapper _mapper;
    private readonly ILogger<TicketService> _logger;

    // Allowed transitions through the generic status endpoint (Day 3)
    private static readonly Dictionary<TicketStatus, TicketStatus[]> AllowedTransitions = new()
    {
        [TicketStatus.Open] = new[] { TicketStatus.InProgress },
        [TicketStatus.InProgress] = new[] { TicketStatus.Resolved },
        [TicketStatus.Resolved] = Array.Empty<TicketStatus>(), // only moves via /close or /return (Day 4)
        [TicketStatus.Closed] = Array.Empty<TicketStatus>()
    };

    public TicketService(
        IGenericRepository<Ticket> repo,
        IGenericRepository<Category> categoryRepo,
        TicketDeskDbContext db,
        IMapper mapper,
        ILogger<TicketService> logger)
    {
        _repo = repo;
        _categoryRepo = categoryRepo;
        _db = db;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TicketDto>> GetAllAsync()
    {
        var tickets = await _repo.GetAllWithIncludesAsync(nameof(Ticket.Categories));
        return _mapper.Map<IEnumerable<TicketDto>>(tickets);
    }

    public async Task<TicketDto?> GetByIdAsync(int id)
    {
        var ticket = await _repo.GetByIdWithIncludesAsync(id, nameof(Ticket.Categories));
        return ticket is null ? null : _mapper.Map<TicketDto>(ticket);
    }

    public async Task<TicketDto> CreateAsync(CreateTicketDto dto)
    {
        var ticket = _mapper.Map<Ticket>(dto);

        var allCategories = await _categoryRepo.GetAllAsync();
        ticket.Categories = allCategories.Where(c => dto.CategoryIds.Contains(c.Id)).ToList();

        await _repo.AddAsync(ticket);
        await _repo.SaveChangesAsync();

        _logger.LogInformation("Ticket {TicketId} created by user {UserId}", ticket.Id, ticket.UserId);

        return _mapper.Map<TicketDto>(ticket);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTicketDto dto)
    {
        var ticket = await _repo.GetByIdWithIncludesAsync(id, nameof(Ticket.Categories));
        if (ticket is null)
        {
            _logger.LogWarning("Update failed — ticket {TicketId} not found", id);
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }

        _mapper.Map(dto, ticket);

        var selectedCategories = await _categoryRepo.GetAllAsync();
        var newCategories = selectedCategories.Where(c => dto.CategoryIds.Contains(c.Id)).ToList();

        ticket.Categories.Clear();
        foreach (var category in newCategories)
        {
            ticket.Categories.Add(category);
        }

        _repo.Update(ticket);
        await _repo.SaveChangesAsync();

        _logger.LogInformation("Ticket {TicketId} updated", id);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ticket = await _repo.GetByIdAsync(id);
        if (ticket is null)
        {
            _logger.LogWarning("Delete failed — ticket {TicketId} not found", id);
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }

        _repo.Delete(ticket);
        await _repo.SaveChangesAsync();

        _logger.LogInformation("Ticket {TicketId} deleted", id);

        return true;
    }

    // Day 3: Open -> InProgress and InProgress -> Resolved
    public async Task ChangeStatusAsync(int id, string newStatusRaw, int currentUserId, string currentUserRole)
    {
        var ticket = await _db.Tickets.FindAsync(id);
        if (ticket is null)
            throw new NotFoundException($"Ticket with id {id} was not found.");

        if (!Enum.TryParse<TicketStatus>(newStatusRaw, ignoreCase: true, out var newStatus))
            throw new InvalidOperationException($"'{newStatusRaw}' is not a valid ticket status.");

        var allowed = AllowedTransitions.TryGetValue(ticket.Status, out var next) ? next : Array.Empty<TicketStatus>();
        if (!allowed.Contains(newStatus))
            throw new InvalidOperationException($"Cannot move a ticket from {ticket.Status} to {newStatus}.");

        // A Manager must be assigned to this specific ticket; Agent/Admin are allowed
        if (currentUserRole == "Manager")
        {
            var isAssigned = await _db.TicketManagers.AnyAsync(tm => tm.TicketId == id && tm.ManagerId == currentUserId);
            if (!isAssigned)
                throw new UnauthorizedAccessException("You are not assigned as a manager on this ticket.");
        }
        else if (currentUserRole != "Agent" && currentUserRole != "Admin")
        {
            throw new UnauthorizedAccessException("You are not permitted to change this ticket's status.");
        }

        ticket.Status = newStatus;

        if (newStatus == TicketStatus.Resolved)
        {
            ticket.ResolvedByUserId = currentUserId;
            ticket.ResolvedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("Ticket {TicketId} status changed to {NewStatus} by user {UserId}", id, newStatus, currentUserId);
    }

    // Day 4: the Agent (checker) finalizes — Resolved -> Closed
    public async Task CloseAsync(int id, int currentUserId)
    {
        var ticket = await _db.Tickets.FindAsync(id);
        if (ticket is null)
            throw new NotFoundException($"Ticket with id {id} was not found.");

        if (ticket.Status != TicketStatus.Resolved)
            throw new InvalidOperationException("Only a Resolved ticket can be closed.");

        ticket.Status = TicketStatus.Closed;
        ticket.ClosedByUserId = currentUserId;
        ticket.ClosedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        _logger.LogInformation("Ticket {TicketId} closed by user {UserId}", id, currentUserId);
    }

    // Day 4: the Agent (checker) sends it back — Resolved -> InProgress
    public async Task ReturnAsync(int id, int currentUserId)
    {
        var ticket = await _db.Tickets.FindAsync(id);
        if (ticket is null)
            throw new NotFoundException($"Ticket with id {id} was not found.");

        if (ticket.Status != TicketStatus.Resolved)
            throw new InvalidOperationException("Only a Resolved ticket can be returned.");

        ticket.Status = TicketStatus.InProgress;
        ticket.ReturnedByUserId = currentUserId;
        ticket.ReturnedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        _logger.LogInformation("Ticket {TicketId} returned to InProgress by user {UserId}", id, currentUserId);
    }
}