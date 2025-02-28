using BackendServer.Data;
using BackendServer.Extensions;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.ProblemModels.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BackendServer.Services.ProblemServices.Repository;

public class ProblemRepository(ApiDbContext context) : IProblemRepository
{
    public Task<Problem?> GetProblemById(Guid id)
    {
        return context.Problems.FirstOrDefaultAsync(p => p.Id == id);
    }

    public ProblemDTO CreateProblem(NewProblem newProblem)
    {
        var problem = new Problem
        {
            Title = newProblem.Title,
            Description = newProblem.Description,
            Id = Guid.NewGuid(),
        };
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
        };
    }

    public void DeleteProblem(Problem problem)
    {
        context.Problems.Remove(problem);
        context.SaveChanges();
    }
}