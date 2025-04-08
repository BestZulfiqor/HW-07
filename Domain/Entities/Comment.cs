using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    [MaxLength(300)] [Required]
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }

    // navigations
    public User User { get; set; }
    public Post Post { get; set; }
}