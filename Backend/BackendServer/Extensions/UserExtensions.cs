using BackendServer.Models.UserModels;
using BackendServer.Models.UserModels.DTOs;

namespace BackendServer.Extensions;

public static class UserExtensions
{
    public static UserDTO ToDTO(this User user)
    {
        return new UserDTO
        {
            Username = user.UserName,SessionToken = user.SessionToken,
            Problems = user.Problems.Select(problem => problem.ToDTO()).ToList(),
            karma = user.Karma,
        };
    }

    public static PublicUserDTO ToPublicDTO(this User user)
    {
        return new PublicUserDTO
        {
            Username = user.UserName,
            Karma = user.Karma,
            Problems = user.Problems.Select(problem => problem.ToDTO()).ToList()
        };
    }
}