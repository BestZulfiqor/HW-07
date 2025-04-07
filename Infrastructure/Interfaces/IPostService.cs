using Domain.DTOs;
using Domain.DTOs.Comments;
using Domain.DTOs.Posts;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IPostService
{
    Task<Response<List<GetPostDto>>> GetAllPostsAsyncTask();
    Task<Response<GetPostDto>> GetPostByIdAsync(int id);
    Task<Response<GetPostDto>> CreatePostAsync(CreatePostDto postDto);
    Task<Response<GetPostDto>> UpdatePostAsync(int id, CreatePostDto postDto);
    Task<Response<string>> DeletePostAsync(int id);
    Task<Response<List<GetPostDto>>> Get5PostsAsync();
    Task<Response<List<PostAuthorDto>>> PostAuthorAsync(int id);
}