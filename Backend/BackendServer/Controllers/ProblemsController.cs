using BackendServer.Extensions;
using BackendServer.Models.ProblemModels.DTOs;
using BackendServer.Services.ProblemServices.Factory;
using BackendServer.Services.ProblemServices.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BackendServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ProblemsController(IProblemRepository problemRepository, IProblemFactory problemFactory) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<ProblemDTO>> GetProblemById(Guid id)
    {
        var problem = await problemRepository.GetProblemById(id);
        if (problem == null) return NotFound("Problem not found");
        return Ok(problem.ToDTO());
    }

    [HttpPost]
    public async Task<ActionResult<ProblemDTO>> PostProblem([FromBody] NewProblem newProblem)
    {
        return Ok(problemRepository.CreateProblem(newProblem));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProblem(Guid id)
    {
        var problem = await problemRepository.GetProblemById(id);
        if (problem == null) return NotFound("Problem not found");
        problemRepository.DeleteProblem(problem);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProblemDTO>> UpdateProblem([FromBody] UpdatedProblem updatedProblem, Guid id)
    {
        var problem = await problemRepository.GetProblemById(id);
        if (problem == null) return NotFound("Problem not found");
        var updated = problemFactory.CreateNewUpdatedProblem(updatedProblem, problem);
        return Ok(problemRepository.UpdateProblem(updated));
    }
}