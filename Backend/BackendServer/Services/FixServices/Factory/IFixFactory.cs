using BackendServer.Models.FixModel;
using BackendServer.Models.FixModel.DTOs;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.UserModels;

namespace BackendServer.Services.FixServices.Factory;

public interface IFixFactory
{
    public Fix CreateFix(NewFix newFix, Problem problem, User user);

    public Fix UpdateFix(string newContent, Fix fix);
}