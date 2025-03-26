using System.ComponentModel.DataAnnotations.Schema;
using BackendServer.Models.FixModel;
using BackendServer.Models.UserModels;

namespace BackendServer.Models.ProblemModels;

public class Problem
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Fix> Fixes { get; set; } = new List<Fix>();
    public User User { get; set; }
    public string UserId { get; set; }
    
    //public DateTime PostedAt { get; set; }


    public bool IsFixed()
    {
        return Fixes.Any(fix => fix.Fixed);
    }
    public override string ToString()
    {
        return Description;
    }
}