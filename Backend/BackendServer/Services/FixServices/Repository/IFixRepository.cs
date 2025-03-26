using BackendServer.Models.FixModel;
using BackendServer.Models.FixModel.DTOs;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.UserModels;

namespace BackendServer.Services.FixServices.Repository;

public interface IFixRepository
{
    public IEnumerable<FixDTO> GetAllFixesByProblemId(Guid problemId);
    
    public Task<FixDTO> CreateFix(Fix fix, User user, Problem problem);
    
    public Task<Fix?> GetFixById(Guid fixId);
    
    public Task DeleteFix(Fix fix, User user);

    public Task<FixDTO> UpdateFix(Fix fix);

    public Task<FixDTO> AcceptFix(Fix fix);
    
    //public void VoteFix(Fix fix, int vote);
    
    public IEnumerable<FixDTO> GetFixByContent(string subContent);
    
    public Task<Fix> UnAcceptFix(Fix fix);

    public Task<Problem> GetProblemByFixId(Guid fixId);
}