using Domain.DTOs;
using Domain.DTOs.Posts;
using Domain.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PostController(IPostService service)
{
    [HttpGet]
    public async Task<Response<List<GetPostDto>>> GetPostsAsync()
    {
        var posts = await service.GetAllPostsAsyncTask();
        return posts;
    }

    [HttpGet("{id:int}")]
    public async Task<Response<GetPostDto>> GetPostAsync(int id)
    {
        var post = await service.GetPostByIdAsync(id);
        return post;
    }

    [HttpPost]
    public async Task<Response<GetPostDto>> CreatePostAsync(CreatePostDto createPostDto)
    {
        var post = await service.CreatePostAsync(createPostDto);
        return post;
    }

    [HttpPut("{id:int}")]
    public async Task<Response<GetPostDto>> UpdatePostAsync(int id, UpdatePostDto updatePostDto)
    {
        var post = await service.UpdatePostAsync(id, updatePostDto);
        return post;
    }

    [HttpDelete("{id:int}")]
    public async Task<Response<string>> DeletePostAsync(int id)
    {
        var post = await service.DeletePostAsync(id);
        return post;
    }

    [HttpGet("{topFive}")]
    public async Task<Response<List<GetPostDto>>> GetTopFivePostsAsync()
    {
        var posts = await service.Get5PostsAsync();
        return posts;
    }

    [HttpGet("{PostAuthor:int}")]
    public async Task<Response<List<PostAuthorDto>>> GetPostAuthorAsync(int id)
    {
        var posts = await service.PostAuthorAsync(id);
        return posts;
    }
}