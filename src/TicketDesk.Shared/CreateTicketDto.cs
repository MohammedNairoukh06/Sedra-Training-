namespace TicketDesk.Shared;

public class CreateTicketDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public List<int> CategoryIds { get; set; } = new();
}