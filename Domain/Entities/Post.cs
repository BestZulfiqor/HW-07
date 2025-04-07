using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Post
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    [MaxLength(500)]
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // navigations
    public User User { get; set; }
    public List<Comment> Comments { get; set; } = [];
}