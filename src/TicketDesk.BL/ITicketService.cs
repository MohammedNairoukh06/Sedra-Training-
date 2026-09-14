using TicketDesk.Shared;

namespace TicketDesk.BL;

public interface ITicketService
{
    Task<IEnumerable<TicketDto>> GetAllAsync();
    Task<TicketDto?> GetByIdAsync(int id);
    Task<TicketDto> CreateAsync(CreateTicketDto dto);
    Task<bool> UpdateAsync(int id, UpdateTicketDto dto);
    Task<bool> DeleteAsync(int id);
}