using BackendServer.Models.ProblemModels;
using BackendServer.Models.ProblemModels.DTOs;

namespace BackendServer.Services.ProblemServices.Factory;

public class ProblemFactory : IProblemFactory
{
    public Problem CreateNewUpdatedProblem(UpdatedProblem updatedProblem, Problem problem)
    {
        problem.Title = updatedProblem.Title;
        problem.Description = updatedProblem.Description;
        return problem;
    }
}