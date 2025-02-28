using BackendServer.Models.ProblemModels;
using BackendServer.Models.ProblemModels.DTOs;

namespace BackendServer.Services.ProblemServices.Factory;

public interface IProblemFactory
{
    public Problem CreateNewUpdatedProblem(UpdatedProblem updatedProblem, Problem problem);
}