using Domain.DTOs.Comments;
using Domain.DTOs.Posts;
using Domain.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CommentController (ICommentService service)
{
    [HttpGet]
    public async Task<Response<List<GetCommentDto>>> GetAllCommentsAsync()
    {
        var comments = await service.GetAllCommentsAsync();
        return comments;
    }

    [HttpGet("{id:int}")]
    public async Task<Response<GetCommentDto>> GetCommentByIdAsync(int id)
    {
        var comment = await service.GetCommentByIdAsync(id);
        return comment;
    }

    [HttpPost]
    public async Task<Response<GetCommentDto>> CreateCommentAsync(CreateCommentDto comment)
    {
        var addComment = await service.CreateCommentAsync(comment);
        return addComment;
    }

    [HttpPut("{id:int}")]
    public async Task<Response<GetCommentDto>> UpdateCommentAsync(int id, UpdateCommentDto comment)
    {
        var updateComment = await service.UpdateCommentAsync(id, comment);
        return updateComment;
    }

    [HttpDelete("{id:int}")]
    public async Task<Response<string>> DeleteCommentAsync(int id)
    {
        var deleteComment = await service.DeleteCommentAsync(id);
        return deleteComment;
    }
}