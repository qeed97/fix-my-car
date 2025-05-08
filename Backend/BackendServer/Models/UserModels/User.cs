using BackendServer.Models.FixModel;
using BackendServer.Models.ProblemModels;
using Microsoft.AspNetCore.Identity;

namespace BackendServer.Models.UserModels;

public class User : IdentityUser
{
    public int Karma { get; set; } = 0;
    public List<Guid> Upvotes { get; set; } = [];
    public List<Guid> Downvotes { get; set; } = [];
    public List<Problem> Problems { get; set; } = [];
    public List<Fix> Fixes { get; set; } = [];
    public string SessionToken { get; set; } = string.Empty;
    
}