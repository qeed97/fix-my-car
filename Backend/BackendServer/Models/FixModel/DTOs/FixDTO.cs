using System.ComponentModel.DataAnnotations.Schema;

namespace BackendServer.Models.FixModel.DTOs;

public class FixDTO
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string Username { get; set; }
    public DateTime PostedAt { get; set; }
    public int Votes { get; set; }
    public bool Fixed  { get; set; }
}