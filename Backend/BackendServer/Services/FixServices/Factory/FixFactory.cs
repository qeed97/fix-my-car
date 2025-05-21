using BackendServer.Models.FixModel;
using BackendServer.Models.FixModel.DTOs;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.UserModels;

namespace BackendServer.Services.FixServices.Factory;

public class FixFactory : IFixFactory
{
    public Fix CreateFix(NewFix newFix, Problem problem, User user)
    {
        return new Fix
        {
            Id = Guid.NewGuid(), Content = newFix.Content, User = user,PostedAt = newFix.PostedAt,
            UserId = user.Id, Problem = problem, ProblemId = problem.Id, Votes = 0
        };
    }

    public Fix UpdateFix(string newContent, Fix fix)
    {
        fix.Content = newContent;
        return fix;
    }
}