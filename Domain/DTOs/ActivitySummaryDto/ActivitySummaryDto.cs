using Domain.Entities;

namespace Domain.DTOs.ActivitySummaryDto;

public class ActivitySummaryDto
{
    public string UserName { get; set; }
    public List<PostActivity> Posts { get; set; }
    public List<CommentActivity> Comments { get; set; }
}