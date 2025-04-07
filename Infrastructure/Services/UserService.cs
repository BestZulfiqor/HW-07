using System.Net;
using Domain.DTOs;
using Domain.DTOs.Users;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

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
}