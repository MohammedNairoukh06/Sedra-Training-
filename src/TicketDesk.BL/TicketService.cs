using AutoMapper;
using TicketDesk.DAL;
using TicketDesk.Domain.Entities;
using TicketDesk.Shared;

namespace TicketDesk.BL;

public class TicketService : ITicketService
{
    private readonly IGenericRepository<Ticket> _repo;
    private readonly IGenericRepository<Category> _categoryRepo;
    private readonly IMapper _mapper;

    public TicketService(IGenericRepository<Ticket> repo, IGenericRepository<Category> categoryRepo, IMapper mapper)
    {
        _repo = repo;
        _categoryRepo = categoryRepo;
        _mapper = mapper;
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

        if (ticket.UserId == 0)
        {
            ticket.UserId = dto.UserId;
        }

        var allCategories = await _categoryRepo.GetAllAsync();
        ticket.Categories = allCategories
            .Where(c => dto.CategoryIds != null && dto.CategoryIds.Contains(c.Id))
            .ToList();

        await _repo.AddAsync(ticket);
        await _repo.SaveChangesAsync();

        return _mapper.Map<TicketDto>(ticket);
    }
    public async Task<bool> UpdateAsync(int id, UpdateTicketDto dto)
    {
        // Load WITH categories, so EF tracks the existing relationships correctly
        var ticket = await _repo.GetByIdWithIncludesAsync(id, nameof(Ticket.Categories));
        if (ticket is null) return false;

        _mapper.Map(dto, ticket);

        var selectedCategories = await _categoryRepo.GetAllAsync();
        var newCategories = selectedCategories.Where(c => dto.CategoryIds.Contains(c.Id)).ToList();

        // Clear and re-add on the TRACKED collection, so EF computes the correct diff
        ticket.Categories.Clear();
        foreach (var category in newCategories)
        {
            ticket.Categories.Add(category);
        }

        _repo.Update(ticket);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ticket = await _repo.GetByIdAsync(id);
        if (ticket is null) return false;

        _repo.Delete(ticket);
        await _repo.SaveChangesAsync();
        return true;
    }
}