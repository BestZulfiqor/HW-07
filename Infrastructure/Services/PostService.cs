using System.Net;
using Domain.DTOs;
using Domain.DTOs.Comments;
using Domain.DTOs.Posts;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class PostService(DataContext context) : IPostService
{
    public async Task<Response<List<GetPostDto>>> GetAllPostsAsyncTask()
    {
        var posts = await context.Posts.ToListAsync();
        var data = posts.Select(p => new GetPostDto
        {
            Id = p.Id,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            UserId = p.UserId,
        }).ToList();

        return new Response<List<GetPostDto>>(data);
    }

    public async Task<Response<GetPostDto>> GetPostByIdAsync(int id)
    {
        var post = await context.Posts.FindAsync(id);
        if (post == null)
        {
            return new Response<GetPostDto>(HttpStatusCode.BadRequest, $"Post with id: {id} does not exist");
        }

        var postDto = new GetPostDto
        {
            Id = post.Id,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UserId = post.UserId,
        };
        return new Response<GetPostDto>(postDto);
    }

    public async Task<Response<GetPostDto>> CreatePostAsync(CreatePostDto postDto)
    {
        var post = new Post()
        {
            UserId = postDto.UserId,
            Content = postDto.Content,
            CreatedAt = postDto.CreatedAt,
        };
        await context.Posts.AddAsync(post);
        var result = await context.SaveChangesAsync();
        var getPostDto = new GetPostDto()
        {
            Content = post.Content,
            CreatedAt = postDto.CreatedAt,
            UserId = postDto.UserId,
        };

        return result == 0
            ? new Response<GetPostDto>(HttpStatusCode.BadRequest, "Post not created")
            : new Response<GetPostDto>(getPostDto);
    }

    public async Task<Response<GetPostDto>> UpdatePostAsync(int id, CreatePostDto postDto)
    {
        var existingPost = await context.Posts.FindAsync(id);
        if (existingPost == null)
        {
            return new Response<GetPostDto>(HttpStatusCode.BadRequest, "Post does not found");
        }

        existingPost.Content = postDto.Content;
        existingPost.CreatedAt = postDto.CreatedAt;
        existingPost.UserId = postDto.UserId;

        var result = await context.SaveChangesAsync();
        var updatePostDto = new GetPostDto()
        {
            Id = existingPost.Id,
            Content = existingPost.Content,
            CreatedAt = existingPost.CreatedAt,
            UserId = existingPost.UserId,
        };

        return result == 0
            ? new Response<GetPostDto>(HttpStatusCode.BadRequest, "Post not updated")
            : new Response<GetPostDto>(updatePostDto);
    }

    public async Task<Response<string>> DeletePostAsync(int id)
    {
        var post = await context.Posts.FindAsync(id);
        if (post == null)
        {
            return new Response<string>("Post does not exist");
        }

        context.Posts.Remove(post);
        var result = await context.SaveChangesAsync();
        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Post not deleted")
            : new Response<string>("Post deleted successfully");
    }

    public async Task<Response<List<GetPostDto>>> Get5PostsAsync()
    {
        var posts = await context.Posts.OrderByDescending(post => post.CreatedAt).Take(5).ToListAsync();
        var data = posts.Select(n => new GetPostDto()
        {
            Id = n.Id,
            UserId = n.UserId,
            Content = n.Content,
            CreatedAt = n.CreatedAt,
        }).ToList();

        return new Response<List<GetPostDto>>(data);
    }

    public async Task<Response<List<PostAuthorDto>>> PostAuthorAsync(int id)
    {
        var post = await context.Posts
            .Include(p => p.User) // загружаем автора
            .Where(p => p.Id == id)
            .Select(p => new PostAuthorDto
            {
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                UserName = p.User.UserName
            })
            .ToListAsync();
        return new Response<List<PostAuthorDto>>(post);
    }
}