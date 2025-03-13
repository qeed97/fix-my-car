using BackendServer.Models.UserModels;
using BackendServer.Models.UserModels.DTOs;

namespace BackendServer.Services.UserServices.Factory;

public class UserFactory : IUserFactory
{
    public User CreateUser(NewUser newUser)
    {
        return new User
        {
            UserName = newUser.Username,
            Karma = 0,
            Email = newUser.Email,
        };
    }
}