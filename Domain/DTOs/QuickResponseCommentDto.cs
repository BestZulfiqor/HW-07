namespace Domain.DTOs;

public class QuickResponseCommentDto
{
    public string Text { get; set; }
    public string UserName { get; set; }
    public int PostId { get; set; }
    public TimeSpan TimeDifferent { get; set; }
}