using Domain.DTOs;
using Domain.DTOs.Posts;
using Domain.DTOs.Users;
using Domain.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service)
{
    [HttpGet]
    public async Task<Response<List<GetUserDto>>> GetUserAsync()
    {
        var users = await service.GetAllUsersAsync();
        return users;
    }

    [HttpGet("{id:int}")]
    public async Task<Response<GetUserDto>> GetPostAsync(int id)
    {
        var user = await service.GetUserByIdAsync(id);
        return user;
    }

    [HttpPost]
    public async Task<Response<GetUserDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = await service.CreateUserAsync(createUserDto);
        return user;
    }

    [HttpPut("{id:int}")]
    public async Task<Response<GetUserDto>> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await service.UpdateUserAsync(id, updateUserDto);
        return user;
    }

    [HttpDelete("{id:int}")]
    public async Task<Response<string>> DeleteUserAsync(int id)
    {
        var user = await service.DeleteUserAsync(id);
        return user;
    }

    [HttpGet("{userId:int}")]
    public async Task<Response<List<UserPostsDto>>> GetUserPostsAsync(int id)
    {
        var user = await service.GetUserPostsAsync(id);
        return user;
    }
}