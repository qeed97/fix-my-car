using System.ComponentModel.DataAnnotations.Schema;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.UserModels;

namespace BackendServer.Models.FixModel;

public class Fix
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }
    public string Content { get; set; }
    public Problem Problem { get; set; }
    public int Votes { get; set; }
    public User User { get; set; }
    public string UserId { get; set; }
    [Column(TypeName = "char(36)")]
    public Guid ProblemId { get; set; }
    //public DateTime PostedAt { get; set; }
    public bool Fixed { get; set; } = false;


    public override string ToString()
    {
        return Content;
    }
}