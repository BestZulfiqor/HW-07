using System.Net;
using Domain.DTOs.Comments;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CommentService(DataContext context) : ICommentService
{
    public async Task<Response<List<GetCommentDto>>> GetAllCommentsAsync()
    {
        var comments = await context.Comments.ToListAsync();

        var data = comments.Select(c => new GetCommentDto()
        {
            Id = c.Id,
            Text = c.Text,
            CreatedAt = c.CreatedAt,
            UserId = c.UserId,
            PostId = c.PostId,
        }).ToList();

        return new Response<List<GetCommentDto>>(data);
    }

    public async Task<Response<GetCommentDto>> GetCommentByIdAsync(int id)
    {
        var comment = await context.Comments.FindAsync(id);
        if (comment == null)
        {
            return new Response<GetCommentDto>(HttpStatusCode.BadRequest, "Comment not found");
        }

        var getCommentDto = new GetCommentDto()
        {
            Id = comment.Id,
            Text = comment.Text,
            CreatedAt = comment.CreatedAt,
            UserId = comment.UserId,
            PostId = comment.PostId,
        };
        
        return new Response<GetCommentDto>(getCommentDto);
    }

    public async Task<Response<GetCommentDto>> CreateCommentAsync(CreateCommentDto commentDto)
    {
        var comment = new Comment()
        {
            UserId = commentDto.UserId,
            PostId = commentDto.PostId,
            Text = commentDto.Text,
            CreatedAt = commentDto.CreatedAt,
        };
        
        await context.Comments.AddAsync(comment);
        var result = await context.SaveChangesAsync();

        var getCommentDto = new GetCommentDto()
        {
            Id = comment.Id,
            Text = comment.Text,
            CreatedAt = comment.CreatedAt,
            UserId = comment.UserId,
            PostId = comment.PostId,
        };

        return result == 0
            ? new Response<GetCommentDto>(HttpStatusCode.BadRequest, "Comment not created")
            : new Response<GetCommentDto>(getCommentDto);
    }

    public async Task<Response<GetCommentDto>> UpdateCommentAsync(int id, UpdateCommentDto commentDto)
    {
        var existingComment = await context.Comments.FindAsync(id);
        if (existingComment == null)
        {
            return new Response<GetCommentDto>(HttpStatusCode.BadRequest, "Comment not found");
        }

        existingComment.Text = commentDto.Text;
        existingComment.CreatedAt = commentDto.CreatedAt;
        existingComment.UserId = commentDto.UserId;
        existingComment.PostId = commentDto.PostId;
            
        var result = await context.SaveChangesAsync();

        var getComment = new GetCommentDto()
        {
            Id = existingComment.Id,
            Text = existingComment.Text,
            CreatedAt = existingComment.CreatedAt,
            UserId = existingComment.UserId,
            PostId = existingComment.PostId,
        };
        return result == 0
            ? new Response<GetCommentDto>(HttpStatusCode.BadRequest, "Comment not updated")
            : new Response<GetCommentDto>(getComment);
    }

    public async Task<Response<string>> DeleteCommentAsync(int id)
    {
        var comment = await context.Comments.FindAsync(id);
        if (comment == null)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Comment not found");    
        }
        context.Comments.Remove(comment);
        var result = await context.SaveChangesAsync();
        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Comment not deleted")
            : new Response<string>("Comment deleted successfully");
    }
}