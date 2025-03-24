using BackendServer.Models.ProblemModels;
using BackendServer.Models.ProblemModels.DTOs;
using BackendServer.Models.UserModels;

namespace BackendServer.Services.ProblemServices.Repository;

public interface IProblemRepository
{
    public Task<bool> CheckIfProblemExists(Guid id);
    public Task<Problem?> GetProblemById(Guid id);
    public ProblemDTO CreateProblem(NewProblem newProblem, User user);
    public ProblemDTO UpdateProblem(Problem problem);
    public void DeleteProblem(Problem problem, User user);
    public IEnumerable<ProblemDTO> GetTenProblems(int startindex);

}