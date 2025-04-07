using Domain.DTOs;
using Domain.DTOs.Users;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<Response<List<GetUserDto>>> GetAllUsersAsync();
    Task<Response<GetUserDto>> GetUserByIdAsync(int id);
    Task<Response<GetUserDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<Response<GetUserDto>> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    Task<Response<string>> DeleteUserAsync(int id);
    Task<Response<List<UserPostsDto>>> GetUserPostsAsync(int id);
}