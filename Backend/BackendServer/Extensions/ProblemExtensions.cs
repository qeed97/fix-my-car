using BackendServer.Models.ProblemModels;
using BackendServer.Models.ProblemModels.DTOs;

namespace BackendServer.Extensions;

public static class ProblemExtensions
{
    public static ProblemDTO ToDTO(this Problem problem)
    {
        return new ProblemDTO
        {
            Id = problem.Id,
            Title = problem.Title,
            Description = problem.Description,
            IsFixed = problem.IsFixed(),
            //PostedAt = problem.PostedAt,
        };
    }

    public static UpdatedProblem ToUpdatedDTO(this Problem problem)
    {
        return new UpdatedProblem
        {
            Title = problem.Title,
            Description = problem.Description,
        };
    }
}