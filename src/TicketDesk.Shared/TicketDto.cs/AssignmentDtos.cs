namespace TicketDesk.Shared;

public class AssignAgentDto
{
    public int AgentId { get; set; }
}

public class AssignManagerDto
{
    public int ManagerId { get; set; }
}

public class ReturnTicketDto
{
    public string Reason { get; set; } = string.Empty;
}

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}