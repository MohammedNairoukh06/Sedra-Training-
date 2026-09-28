using TicketDesk.Shared;

namespace TicketDesk.BL;

public interface ITicketService
{
    Task<IEnumerable<TicketDto>> GetAllAsync();
    Task<TicketDto?> GetByIdAsync(int id);
    Task<TicketDto> CreateAsync(CreateTicketDto dto);
    Task<bool> UpdateAsync(int id, UpdateTicketDto dto);
    Task<bool> DeleteAsync(int id);
    Task ChangeStatusAsync(int id, string newStatus, int currentUserId, string currentUserRole);
    Task CloseAsync(int id, int currentUserId);
    Task ReturnAsync(int id, int currentUserId);
}