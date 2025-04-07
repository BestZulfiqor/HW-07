using Domain.DTOs.Comments;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface ICommentService
{ 
    Task<Response<List<GetCommentDto>>> GetAllCommentsAsync();
    Task<Response<GetCommentDto>> GetCommentByIdAsync(int id);
    Task<Response<GetCommentDto>> CreateCommentAsync(CreateCommentDto commentDto);
    Task<Response<GetCommentDto>> UpdateCommentAsync(int id, UpdateCommentDto commentDto);
    Task<Response<string>> DeleteCommentAsync(int id);
}