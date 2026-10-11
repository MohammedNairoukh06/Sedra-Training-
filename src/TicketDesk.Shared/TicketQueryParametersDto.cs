namespace TicketDesk.Shared;

public class TicketQueryParameters
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? 1 : value);
    }

    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? SortBy { get; set; } = "createdAt_desc";
}