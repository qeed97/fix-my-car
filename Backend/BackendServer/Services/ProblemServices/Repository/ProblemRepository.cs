using BackendServer.Data;
using BackendServer.Extensions;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.ProblemModels.DTOs;
using BackendServer.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace BackendServer.Services.ProblemServices.Repository;

public class ProblemRepository(ApiDbContext context) : IProblemRepository
{
    public Task<Problem?> GetProblemById(Guid id)
    {
        return context.Problems
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public ProblemDTO CreateProblem(NewProblem newProblem, User user)
    {
        var problem = new Problem
        {
            Title = newProblem.Title,
            Description = newProblem.Description,
            User = user,
            UserId = user.Id,
            Id = Guid.NewGuid(),
        };
        user.Problems.Add(problem);
        context.Problems.Add(problem);
        context.SaveChanges();
        return problem.ToDTO();
    }

    public ProblemDTO UpdateProblem(Problem problem)
    {
        context.Problems.Update(problem);
        context.SaveChanges();
        return new ProblemDTO
        {
            Id = problem.Id,
            Title = problem.Title,
            Description = problem.Description,
            Username = problem.User.UserName,
        };
    }

    public void DeleteProblem(Problem problem, User user)
    {
        user.Problems.Remove(problem);
        context.Problems.Remove(problem);
        context.SaveChanges();
        
    }

    public IEnumerable<ProblemDTO> GetTenProblems(int startindex)
    {
        return context.Problems
            //.Include(p => p.Fixes)
            .Include(p => p.User)
            .Skip(startindex).Take(startindex + 10).Select(p => p.ToDTO());
    }
}