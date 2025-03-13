using System.Security.Claims;
using BackendServer.Extensions;
using BackendServer.Models.UserModels.DTOs;
using BackendServer.Services.AuthenticationServices.TokenService;
using BackendServer.Services.UserServices.Factory;
using BackendServer.Services.UserServices.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BackendServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserRepository userRepository, IUserFactory userFactory, ITokenService tokenService)
    : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult<UserDTO>> CreateUserAndLogin([FromBody] NewUser newUser)
    {
        if (await userRepository.AreCredentialsTaken(newUser.Email, newUser.Username))
        {
            throw new Exception("your email and/or username are taken");
        }

        var user = userFactory.CreateUser(newUser);
        await userRepository.CreateUser(user, newUser.Password, "User");
        user.SessionToken = await userRepository.LoginUser(newUser.Email, newUser.Password);
        return Ok(user.ToDTO());
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginCredentials loginCredentials)
    {
        var token = await userRepository.LoginUser(loginCredentials.Email, loginCredentials.Password);
        return Content($"\"{token}\"", "application/json");
    }

    [HttpPost("logout")]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult> LogoutUser()
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("this token is invalid");
        await userRepository.LogoutUser(username);
        return Ok();
    }

    [HttpGet("GetBySessionToken")]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<UserDTO>> GetUser()
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("this token is invalid");
        var user = await userRepository.GetUserByUsername(username) ?? throw new Exception("user not found");
        return user.ToDTO();
    }

    [HttpGet("IsUserAdmin")]
    [Authorize(Roles = "Admin, User")]
    public ActionResult<bool> IsUserAdmin()
    {
        return Ok(User.IsInRole("Admin"));
    }
    
}