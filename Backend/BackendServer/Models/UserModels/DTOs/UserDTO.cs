using BackendServer.Models.FixModel.DTOs;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.ProblemModels.DTOs;

namespace BackendServer.Models.UserModels.DTOs;

public class UserDTO
{
    public string Username { get; set; }
    public int karma { get; set; } = 0;
    public List<ProblemDTO> Problems { get; set; } 
    public List<FixDTO> Fixes { get; set; } 
    public string SessionToken { get; set; }

}