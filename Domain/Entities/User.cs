using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    [Required] [MaxLength(50)]
    public string UserName { get; set; }
    [MaxLength(100)]
    public string Email { get; set; }

    public DateTime JoinDate { get; set; }
    [MaxLength(200)]
    public string Bio { get; set; }
    
    // navigations
    public List<Post> Posts { get; set; } = [];
    public List<Comment> Comments { get; set; }
}