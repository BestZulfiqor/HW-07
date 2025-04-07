namespace Domain.DTOs.Posts;

public class CreatePostDto
{
    public int UserId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int PostCount { get; set; }
}