using BackendServer.Services.UserServices.Factory;
using BackendServer.Services.UserServices.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BackendServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserRepository userRepository, IUserFactory userFactory/*,ITokenService tokenService*/)
    : ControllerBase
{
    
}