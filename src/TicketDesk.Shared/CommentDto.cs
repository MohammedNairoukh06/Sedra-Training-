using System.ComponentModel.DataAnnotations;

namespace TicketDesk.Shared;

public class CreateCommentDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Comment content cannot be empty.")]
    public string Content { get; set; } = string.Empty;
}

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
}