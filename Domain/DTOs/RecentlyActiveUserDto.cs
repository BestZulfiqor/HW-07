namespace Domain.DTOs;

public class RecentlyActiveUserDto
{
    public string UserName { get; set; }
    public DateTime LastPostDate { get; set; }
    public int PostCount { get; set; }
}