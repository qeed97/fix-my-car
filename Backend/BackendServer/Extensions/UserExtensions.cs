using BackendServer.Models.UserModels;
using BackendServer.Models.UserModels.DTOs;

namespace BackendServer.Extensions;

public static class UserExtensions
{
    public static UserDTO ToDTO(this User user)
    {
        var upvotes = new List<Guid>();
        var downvotes = new List<Guid>();
        
        if (user.Upvotes != null && user.Upvotes.Count != 0)
        {
            upvotes = user.Upvotes.Select(upvote => upvote).ToList();
        }

        if (user.Downvotes != null && user.Downvotes.Count != 0)
        {
            downvotes = user.Downvotes.Select(upvote => upvote).ToList();
        }
        
        return new UserDTO
        {
            Username = user.UserName,SessionToken = user.SessionToken,
            Fixes = user.Fixes.Select(fix => fix.ToDTO()).ToList(),
            Problems = user.Problems.Select(problem => problem.ToDTO()).ToList(),
            Upvotes = upvotes,
            Downvotes = downvotes,
            karma = user.Karma,
        };
    }

    public static PublicUserDTO ToPublicDTO(this User user)
    {
        return new PublicUserDTO
        {
            Username = user.UserName,
            Karma = user.Karma,
            Fixes = user.Fixes.Select(fix => fix.ToDTO()).ToList(),
            Problems = user.Problems.Select(problem => problem.ToDTO()).ToList()
        };
    }
}