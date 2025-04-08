using System.Net;
using Domain;
using Domain.DTOs;
using Domain.DTOs.ActivitySummaryDto;
using Domain.DTOs.Users;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Infrastructure.Services;

public class UserService(DataContext context) : IUserService
{
    public async Task<Response<List<GetUserDto>>> GetAllUsersAsync()
    {
        var users = await context.Users.ToListAsync();

        var data = users.Select(u => new GetUserDto
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            Bio = u.Bio,
        }).ToList();

        return new Response<List<GetUserDto>>(data);
    }

    public async Task<Response<GetUserDto>> GetUserByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return new Response<GetUserDto>(HttpStatusCode.BadRequest, $"User with id {id} not found");
        }

        var userDto = new GetUserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Bio = user.Bio,
        };

        return new Response<GetUserDto>(userDto);
    }

    public async Task<Response<GetUserDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = new User()
        {
            Bio = createUserDto.Bio,
            Email = createUserDto.Email,
            UserName = createUserDto.UserName,
        };
        await context.Users.AddAsync(user);
        var result = await context.SaveChangesAsync();

        var userDto = new GetUserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Bio = user.Bio,
        };

        return result == 0
            ? new Response<GetUserDto>(HttpStatusCode.BadRequest, "User not created")
            : new Response<GetUserDto>(userDto);
    }

    public async Task<Response<GetUserDto>> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var existingUser = await context.Users.FindAsync(id);
        if (existingUser == null)
        {
            return new Response<GetUserDto>(HttpStatusCode.BadRequest, $"User with id {id} not found");
        }

        existingUser.Bio = updateUserDto.Bio;
        existingUser.Email = updateUserDto.Email;
        existingUser.UserName = updateUserDto.UserName;
        var result = await context.SaveChangesAsync();

        var updateUser = new GetUserDto()
        {
            Id = existingUser.Id,
            UserName = existingUser.UserName,
            Email = existingUser.Email,
            Bio = existingUser.Bio,
        };
        return result == 0
            ? new Response<GetUserDto>(HttpStatusCode.BadRequest, "User not updated")
            : new Response<GetUserDto>(updateUser);
    }

    Task<Response<string>> IUserService.DeleteUserAsync(int id)
    {
        return DeleteUserAsync(id);
    }

    public async Task<Response<string>> DeleteUserAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return new Response<string>("User does not exist");
        }

        context.Remove(user);
        var result = await context.SaveChangesAsync();
        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "User not deleted")
            : new Response<string>(HttpStatusCode.OK, "User deleted");
    }

    public async Task<Response<List<UserPostsDto>>> GetUserPostsAsync(int id)
    {
        var posts = await context.Posts.Where(u => u.UserId == id).Include(p => p.User).Select(up => new UserPostsDto()
        {
            Content = up.Content,
            CreatedAt = up.CreatedAt,
            UserName = up.User.UserName,
        }).ToListAsync();

        return new Response<List<UserPostsDto>>(posts);
    }

    // Task 1
    public async Task<Response<List<NewRegistrationDto>>> GetNewRegistrationAsync()
    {
        var now = DateTime.Now;

        var users = await context.Users
            .Where(u => now.AddDays(-14) <= u.JoinDate).ToListAsync();

        var data = users.Select(u => new NewRegistrationDto()
        {
            UserName = u.UserName,
            Email = u.Email,
            JoinDate = u.JoinDate,
        }).ToList();
        return new Response<List<NewRegistrationDto>>(data);
    }

    // 2
    public async Task<Response<List<ActivePosterDto>>> GetActivePoster()
    {
        var users = await context.Users.Include(n => n.Posts)
            .Where(u => u.Posts.Any())
            .ToListAsync();

        var usersDto = users.Select(u => new ActivePosterDto()
        {
            UserName = u.UserName,
            PostCount = u.Posts.Count,
        }).ToList();

        return new Response<List<ActivePosterDto>>(usersDto.ToList());
    }

    // 3
    public async Task<Response<List<RecentlyActiveUserDto>>> GetRecentlyActiveUsers()
    {
        var usersDto = await context.Posts
            .Where(p => p.CreatedAt.AddDays(7) >= DateTime.Now)
            .GroupBy(p => p.User)
            .Select(g => new RecentlyActiveUserDto
            {
                UserName = g.Key.UserName,
                PostCount = g.Count(),
                LastPostDate = g.Max(p => p.CreatedAt)
            })
            .ToListAsync();

        return new Response<List<RecentlyActiveUserDto>>(usersDto);
    }

    // 4
    public async Task<Response<List<TopCreatorDto>>> GetTopCreators()
    {
        var users = await context.Users
            .Include(n => n.Posts)
            .OrderByDescending(u => u.Posts.Count)
            .Take(5)
            .Select(u => new TopCreatorDto()
            {
                UserName = u.UserName,
                PostCount = u.Posts.Count,
            }).ToListAsync();
        return new Response<List<TopCreatorDto>>(users);
    }

    // 5
    public async Task<Response<List<HighInteractionUserDto>>> GetHighInteractionUsers()
    {
        var users = await context.Users
            .Include(u => u.Posts)
            .ThenInclude(n => n.Comments)
            .Where(n => n.Posts.Any())
            .Select(n => new HighInteractionUserDto()
            {
                UserName = n.UserName,
                PostCount = n.Posts.Count,
                AvgCommentPerPost = n.Posts.Average(p => p.Comments.Count),
            })
            .Where(n => n.AvgCommentPerPost > 5)
            .ToListAsync();

        return new Response<List<HighInteractionUserDto>>(users);
    }

    // 6
    public async Task<Response<List<LatestPostDto>>> GetLatestPosts()
    {
        var users = await context.Posts
            .OrderByDescending(n => n.CreatedAt)
            .Take(5)
            .Select(n => new LatestPostDto()
            {
                Content = n.Content,
                CreatedAt = n.CreatedAt,
                UserName = n.User.UserName,
            }).ToListAsync();

        return new Response<List<LatestPostDto>>(users);
    }

    // 7
    public async Task<Response<List<UserRecentPostDto>>> GetUserRecentPosts(int userId)
    {
        var posts = await context.Posts
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(5)
            .Select(n => new UserRecentPostDto()
            {
                Content = n.Content,
                CreatedAt = n.CreatedAt,
                UserName = n.User.UserName,
            }).ToListAsync();
        return new Response<List<UserRecentPostDto>>(posts);
    }

    // 8
    public async Task<Response<List<HighCommentPostDto>>> GetHighCommentPosts()
    {
        var posts = await context.Posts
            .Where(n => n.Comments.Count > 10)
            .Select(n => new HighCommentPostDto()
            {
                Content = n.Content,
                CommentCount = n.Comments.Count,
                UserName = n.User.UserName,
            }).ToListAsync();
        return new Response<List<HighCommentPostDto>>(posts);
    }

    // 9
    public async Task<Response<List<RecentCommentDto>>> GetRecentComments()
    {
        var comments = await context.Comments
            .OrderByDescending(n => n.CreatedAt)
            .Take(5)
            .Select(n => new RecentCommentDto()
            {
                CreatedAt = n.CreatedAt,
                Text = n.Text,
                UserName = n.User.UserName,
            }).ToListAsync();
        return new Response<List<RecentCommentDto>>(comments);
    }

    // 10
    public async Task<Response<List<PostRecentCommentsDto>>> GetPostRecentComments(int id)
    {
        var comments = await context.Comments
            .OrderByDescending(n => n.CreatedAt)
            .Where(p => p.Post.Id == id)
            .Take(5)
            .Select(n => new PostRecentCommentsDto
            {
                CreatedAt = n.CreatedAt,
                Text = n.Text,
                UserName = n.User.UserName,
            }).ToListAsync();
        return new Response<List<PostRecentCommentsDto>>(comments);
    }

    // 11
    public async Task<Response<List<LongTextCommentDto>>> GetLongTextComments()
    {
        var comments = await context.Comments
            .Where(n => n.Text.Length >= 200)
            .Select(n => new LongTextCommentDto()
            {
                Text = n.Text,
                TextLength = n.Text.Length,
                UserName = n.User.UserName,
            }).ToListAsync();
        return new Response<List<LongTextCommentDto>>(comments);
    }

    // 12
    public async Task<Response<List<QuickResponseCommentDto>>> GetQuickResponseComments()
    {
        var comments = await context.Comments
            .Where(n => n.CreatedAt <= n.Post.CreatedAt.AddMinutes(15))
            .Select(n => new QuickResponseCommentDto()
            {
                Text = n.Text,
                PostId = n.Post.Id,
                UserName = n.User.UserName,
                TimeDifferent = n.CreatedAt - n.Post.CreatedAt
            }).ToListAsync();

        return new Response<List<QuickResponseCommentDto>>(comments);
    }

    // 13
    public async Task<Response<List<ActivitySummaryDto>>> GetActivitySummary(int id)
    {
        var userActivity = await context.Users
            .Where(n => n.Id == id)
            .Select(n => new ActivitySummaryDto
            {
                UserName = n.UserName,
                Posts = n.Posts
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(3)
                    .Select(p => new PostActivity
                    {
                        Content = p.Content,
                        CreatedAt = p.CreatedAt
                    }).ToList(),
                Comments = n.Comments
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(3)
                    .Select(c => new CommentActivity
                    {
                        Text = c.Text,
                        PostId = c.PostId
                    }).ToList()
            }).ToListAsync();

        return new Response<List<ActivitySummaryDto>>(userActivity);
    }
    
    // 14
    public async Task<Response<List<RecentPopularPostDto>>> GetRecentPopularPosts()
    {
        var posts = await context.Posts
            .Where(n => n.Comments.Count > 5)
            .OrderByDescending(n => n.CreatedAt)
            .Take(5)
            .Select(n => new RecentPopularPostDto
            {
                Content = n.Content,
                CreatedAt = n.CreatedAt,
                CommentCount = n.Comments.Count,
                UserName = n.User.UserName,
            }).ToListAsync();
        
        return new Response<List<RecentPopularPostDto>>(posts);
    }
    
    // 15
    public async Task<Response<List<TopCommenterDto>>> GetTopCommenters()
    {
        var users = await context.Users
            .OrderByDescending(n => n.Comments.Count)
            .Take(5)
            .Select(n => new TopCommenterDto()
            {
                CommentCount = n.Comments.Count,
                UserName = n.UserName
            }).ToListAsync();
        
        return new Response<List<TopCommenterDto>>(users);
    }
}