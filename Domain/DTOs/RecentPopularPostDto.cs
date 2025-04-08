namespace Domain.DTOs;

public class RecentPopularPostDto
{
    public string Content { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CommentCount { get; set; }
}