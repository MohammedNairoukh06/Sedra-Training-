namespace TicketDesk.Shared;

public class UpdateTicketDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<int> CategoryIds { get; set; } = new();
}