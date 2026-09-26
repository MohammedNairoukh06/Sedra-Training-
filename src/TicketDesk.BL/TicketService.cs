using AutoMapper;
using Microsoft.Extensions.Logging;
using TicketDesk.BL.Exceptions;
using TicketDesk.DAL;
using TicketDesk.Domain.Entities;
using TicketDesk.Shared;

namespace TicketDesk.BL;

public class TicketService : ITicketService
{
    private readonly IGenericRepository<Ticket> _repo;
    private readonly IGenericRepository<Category> _categoryRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<TicketService> _logger;

    public TicketService(
        IGenericRepository<Ticket> repo,
        IGenericRepository<Category> categoryRepo,
        IMapper mapper,
        ILogger<TicketService> logger)
    {
        _repo = repo;
        _categoryRepo = categoryRepo;
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
}