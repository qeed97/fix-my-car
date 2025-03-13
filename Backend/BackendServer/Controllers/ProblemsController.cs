using System.Security.Claims;
using BackendServer.Extensions;
using BackendServer.Models.ProblemModels.DTOs;
using BackendServer.Services.ProblemServices.Factory;
using BackendServer.Services.ProblemServices.Repository;
using BackendServer.Services.UserServices.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ProblemsController(IProblemRepository problemRepository, IProblemFactory problemFactory, IUserRepository userRepository) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<ProblemDTO>> GetProblemById(Guid id)
    {
        var problem = await problemRepository.GetProblemById(id);
        if (problem == null) return NotFound("Problem not found");
        return Ok(problem.ToDTO());
    }

    [HttpPost]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<ProblemDTO>> PostProblem([FromBody] NewProblem newProblem)
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? throw new UnauthorizedAccessException("this token is not valid");
        var user = await userRepository.GetUserOnlyProblems(username) ?? throw new Exception("User not found");
        if (!userRepository.IsUserLoggedIn(username)) throw new UnauthorizedAccessException("you are not logged in");
        await userRepository.UpdateKarma(user, 5);
        return Ok(problemRepository.CreateProblem(newProblem, user));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult> DeleteProblem(Guid id)
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? throw new UnauthorizedAccessException("this token is not valid");
        var problem = await problemRepository.GetProblemById(id) ?? throw new Exception("Problem not found");
        var user = await userRepository.GetUserOnlyProblems(username) ?? throw new Exception("User not found");
        if (problem.User.Id != user.Id && !User.IsInRole("Admin")) throw new UnauthorizedAccessException("You do not have permission to delete this problem");
        problemRepository.DeleteProblem(problem, problem.User);
        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<ProblemDTO>> UpdateProblem([FromBody] UpdatedProblem updatedProblem, Guid id)
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? throw new UnauthorizedAccessException("this token is not valid");
        var problem = await problemRepository.GetProblemById(id);
        if (problem == null) return NotFound("Problem not found");
        var user = await userRepository.GetUserOnlyProblems(username) ?? throw new Exception("User not found");
        if (problem.User.Id != user.Id && !User.IsInRole("Admin")) throw new UnauthorizedAccessException("You do not have permission to update this problem");
        var updated = problemFactory.CreateNewUpdatedProblem(updatedProblem, problem);
        return Ok(problemRepository.UpdateProblem(updated));
    }

    [HttpGet]
    public ActionResult<MainPageProblemDTO> GetProblems(int startindex)
    {
        return Ok(new MainPageProblemDTO
        {
            Problems = problemRepository.GetTenProblems(startindex).ToList(),
            Index = startindex + 10
        });
    }
}