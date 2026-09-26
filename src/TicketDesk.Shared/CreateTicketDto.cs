using System.ComponentModel.DataAnnotations;

namespace TicketDesk.Shared;

public class CreateTicketDto
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(1000, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 1000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "UserId is required.")]
    public int UserId { get; set; }

    public List<int> CategoryIds { get; set; } = new();
}