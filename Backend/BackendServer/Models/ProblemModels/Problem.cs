using System.ComponentModel.DataAnnotations.Schema;
using BackendServer.Models.UserModels;

namespace BackendServer.Models.ProblemModels;

public class Problem
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public User User { get; set; }
    public string UserId { get; set; }
    
    //public DateTime PostedAt { get; set; }

    public override string ToString()
    {
        return Description;
    }
}