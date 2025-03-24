using BackendServer.Data;
using BackendServer.Extensions;
using BackendServer.Models.FixModel;
using BackendServer.Models.FixModel.DTOs;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.UserModels;
using FuzzySharp;
using Microsoft.EntityFrameworkCore;

namespace BackendServer.Services.FixServices.Repository;

public class FixRepository(ApiDbContext dbContext) : IFixRepository
{
    public IEnumerable<FixDTO> GetAllFixesByProblemId(Guid problemId)
    {
        return dbContext.Fixes.Where(f => f.ProblemId == problemId)
            .Include(f => f.User)
            .Select(f => f.ToDTO());
    }

    public async Task<FixDTO> CreateFix(Fix fix, User user, Problem problem)
    {
        dbContext.Fixes.Add(fix);
        user.Fixes.Add(fix);
        problem.Fixes.Add(fix);
        await dbContext.SaveChangesAsync();
        return fix.ToDTO();
    }

    public Task<Fix?> GetFixById(Guid fixId)
    {
        return dbContext.Fixes
            .Include(f => f.User)
            .Include(f => f.Problem)
            .FirstOrDefaultAsync(f => f.Id == fixId);
    }

    public async Task DeleteFix(Fix fix, User user)
    {
        dbContext.Remove(fix);
        user.Fixes.Remove(fix);
        fix.Problem.Fixes.Remove(fix);
        await dbContext.SaveChangesAsync();
    }

    public async Task<FixDTO> UpdateFix(Fix fix)
    {
        dbContext.Update(fix);
        await dbContext.SaveChangesAsync();
        return fix.ToDTO();
    }

    public async Task<FixDTO> AcceptFix(Fix fix)
    {
        fix.Fixed = true;
        await dbContext.SaveChangesAsync();
        return fix.ToDTO();
    }

    public IEnumerable<FixDTO> GetFixByContent(string subContent)
    {
        var bestResults = Process.ExtractSorted(subContent, dbContext.Fixes.Select(f => f.Content).ToArray())
            .Select(res => res.Value)
            .Take(10);

        var fixes = dbContext.Fixes.Include(f => f.Problem);
        
        return bestResults
            .Select(content => fixes.FirstOrDefault(f => f.Content == content.ToString()))
            .Select(f => f?.ToDTO() ?? throw new Exception("This fix could not be found."));
    }

    public async Task<Fix> UnAcceptFix(Fix fix)
    {
        var fixes = await dbContext.Fixes
            .FirstOrDefaultAsync(f => f.Id == fix.Id) ?? throw new Exception("This fix could not be found.");
        fix.Fixed = false;
        await dbContext.SaveChangesAsync();
        return fix;
    }

    public async Task<Problem> GetProblemByFixId(Guid fixId)
    {
        return (
            await dbContext.Fixes
                .Include(fix => fix.Problem)
                .FirstOrDefaultAsync(f => f.Id == fixId) ??
            throw new Exception("This fix could not be found.")).Problem;
    }
}