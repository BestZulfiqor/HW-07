namespace Domain.DTOs.Comments;

public class CreateCommentDto
{
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CommentCount { get; set; }
}