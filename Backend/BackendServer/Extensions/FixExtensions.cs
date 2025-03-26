using BackendServer.Models.FixModel;
using BackendServer.Models.FixModel.DTOs;

namespace BackendServer.Extensions;

public static class FixExtensions
{
    public static FixDTO ToDTO(this Fix fix)
    {
        return new FixDTO
        {
            Id = fix.Id,
            Content = fix.Content,
            //PostedAt = fix.PostedAt,
            Username = fix.User.UserName,
            Fixed = fix.Fixed,
            //Votes = fix.Votes
        };
    }
}