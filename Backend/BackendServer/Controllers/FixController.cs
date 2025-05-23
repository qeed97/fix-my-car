﻿using System.Security.Authentication;
using System.Security.Claims;
using BackendServer.Models.FixModel.DTOs;
using BackendServer.Services.FixServices.Factory;
using BackendServer.Services.FixServices.Repository;
using BackendServer.Services.ProblemServices.Repository;
using BackendServer.Services.UserServices.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendServer.Controllers;

[ApiController]
[Route("[controller]")]
public class FixController(
    IFixRepository fixRepository,
    IUserRepository userRepository,
    IProblemRepository problemRepository,
    IFixFactory fixFactory
    ) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FixDTO>>> GetAllFixesForProblem(Guid problemId)
    {
        if (!await problemRepository.CheckIfProblemExists(problemId))
        {
            throw new Exception($"Problem with id {problemId} does not exist");
        }
        return Ok(fixRepository.GetAllFixesByProblemId(problemId));
    }

    [HttpPost]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<FixDTO>> PostNewFixToProblem(Guid problemId, [FromBody] NewFix newFix)
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("this user could not be found");
        var user = await userRepository.GetUserOnlyFixes(username) ?? throw new Exception("this user could not be found");
        var problem = await problemRepository.GetProblemById(problemId) ?? throw new Exception("this problem could not be found");
        
        var fix = fixFactory.CreateFix(newFix, problem, user);
        await userRepository.UpdateKarma(user, 5);
        return Ok(await fixRepository.CreateFix(fix, user, problem));
    }

    [HttpDelete("{fixId:guid}")]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult> DeleteFix(Guid fixId)
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("this user could not be found");
        var user = await userRepository.GetUserOnlyFixes(username) ?? throw new Exception("this user could not be found");
        var fix = await fixRepository.GetFixById(fixId) ?? throw new Exception("this fix could not be found");

        if (user.Id != fix.UserId && !User.IsInRole("Admin"))
        {
            throw new AuthenticationException("You are not authorized to delete this fix");
        }
        await fixRepository.DeleteFix(fix, user);
        return NoContent();
    }

    [HttpPut("{fixId:guid}")]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<FixDTO>> UpdateFix(Guid fixId, [FromBody] string newContent)
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("this user could not be found");
        var user = await userRepository.GetUserOnlyFixes(username) ?? throw new Exception("this user could not be found");
        var fix = await fixRepository.GetFixById(fixId) ?? throw new Exception("this fix could not be found");

        if (user.Id != fix.UserId && !User.IsInRole("Admin"))
        {
            throw new AuthenticationException("You are not authorized to update this fix");
        }

        var newFix = fixFactory.UpdateFix(newContent, fix);
        return Ok(await fixRepository.UpdateFix(newFix));
    }

    [HttpPost("/accept/{fixId:guid}")]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<FixDTO>> AcceptFix(Guid fixId)
    {
        var fix = await fixRepository.GetFixById(fixId) ?? throw new Exception("this fix could not be found");
        var username = User.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("this user could not be found");
        var userId = (await userRepository.GetUserByUsername(username) ?? throw new Exception("this user could not be found")).Id;
        if (fix.Problem.UserId != userId && !User.IsInRole("Admin"))
        {
            throw new AuthenticationException("You are not authorized to accept this fix");
        }

        if (fix.Problem.IsFixed())
        {
            throw new Exception("This Problem is already fixed");
        }

        return Ok(await fixRepository.AcceptFix(fix));
    }

    [HttpGet("problem/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> GetProblemOfFix(Guid id)
    {
        var problem = await fixRepository.GetProblemByFixId(id) ?? throw new Exception("this problem could not be found");
        return Ok(problem.Id);
    }
    
    [HttpPatch("{id:guid}/upvote")]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult> UpVoteFix(Guid id)
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("This token is invalid");
        var user = await userRepository.GetUserOnlyFixes(username) ??
                   throw new Exception("This user could not be found");
        var fix = await fixRepository.GetFixById(id) ??
                  throw new Exception("This fix could not be found");
        var fixUser = fix.User;
        if (user.Upvotes.Contains(fix.Id))
        {
            var unVote = -1;
            await userRepository.RemoveUpvote(user, fix.Id);
            fixRepository.VoteFix(fix, unVote);
            await userRepository.UpdateKarma(fixUser, unVote);

            return Content("\"Unvoted fix\"", "application/json");
        }

        if (user.Downvotes.Contains(fix.Id))
        {
            var reVote = 2;
            await userRepository.RemoveDownvote(user, fix.Id);
            fixRepository.VoteFix(fix, reVote);
            await userRepository.UpdateKarma(fixUser, reVote);
            await userRepository.Upvote(user, fix.Id);

            return Content("\"Upvoted fix\"", "application/json");
        }

        var vote = 1;
        fixRepository.VoteFix(fix, vote);
        await userRepository.UpdateKarma(fixUser, vote);
        await userRepository.Upvote(user, fix.Id);

        return Content("\"Upvoted fix\"", "application/json");
    }

    [HttpPatch("{id:guid}/downvote")]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult> DownVoteFix(Guid id)
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("This token is invalid");
        var user = await userRepository.GetUserOnlyFixes(username) ??
                   throw new Exception("This user could not be found");
        var fix = await fixRepository.GetFixById(id) ??
                     throw new Exception("This fix could not be found");

        var fixUser = fix.User;

        if (user.Downvotes.Contains(fix.Id))
        {
            var unVote = 1;
            await userRepository.RemoveDownvote(user, fix.Id);
            fixRepository.VoteFix(fix, unVote);
            await userRepository.UpdateKarma(fixUser, unVote);

            return Content("\"Unvoted fix\"", "application/json");
        }

        if (user.Upvotes.Contains(fix.Id))
        {
            var reVote = -2;
            await userRepository.RemoveUpvote(user, fix.Id);
            fixRepository.VoteFix(fix, reVote);
            await userRepository.UpdateKarma(fixUser, reVote);
            await userRepository.Downvote(user, fix.Id);
            return Content("\"Downvoted fix\"", "application/json");
        }

        var vote = -1;
        fixRepository.VoteFix(fix, vote);
        await userRepository.UpdateKarma(fixUser, vote);
        await userRepository.Downvote(user, fix.Id);

        return Content("\"Downvote fix\"", "application/json");
    }
}