using BackendServer.Models.ProblemModels;
using BackendServer.Models.ProblemModels.DTOs;

namespace BackendServer.Services.ProblemServices.Repository;

public interface IProblemRepository
{
    public Task<Problem?> GetProblemById(Guid id);
    public ProblemDTO CreateProblem(NewProblem newProblem);
    public ProblemDTO UpdateProblem(Problem problem);
    public void DeleteProblem(Problem problem);
}