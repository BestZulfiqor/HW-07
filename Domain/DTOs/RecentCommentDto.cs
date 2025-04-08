namespace Domain.DTOs;

public class RecentCommentDto
{
    public string Text { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
}