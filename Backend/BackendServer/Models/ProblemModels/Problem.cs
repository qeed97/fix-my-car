namespace BackendServer.Models.ProblemModels;

public class Problem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    //public DateTime PostedAt { get; set; }

    public override string ToString()
    {
        return Description;
    }
}