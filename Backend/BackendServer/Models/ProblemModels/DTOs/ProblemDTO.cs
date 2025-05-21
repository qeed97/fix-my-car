using System.ComponentModel.DataAnnotations.Schema;

namespace BackendServer.Models.ProblemModels.DTOs;

public class ProblemDTO
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Username { get; set; }
    public bool IsFixed  { get; set; }
    public DateTime PostedAt { get; set; }
}