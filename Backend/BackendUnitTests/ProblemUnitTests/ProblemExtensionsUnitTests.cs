using BackendServer.Extensions;
using BackendServer.Models.FixModel;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.UserModels;

namespace BackendUnitTests.ProblemUnitTests;

[TestFixture]
public class ProblemExtensionsUnitTests
{
    [Test]
    public void ToDTO_ConvertsCorrectly()
    {
        var user = new User { Id = "userId", UserName = "userName" };
        var fixes = new List<Fix>();

        var problem = new Problem
        {
            Id = Guid.NewGuid(),
            Description = "This is a test",
            Title = "This is a test",
            Fixes = fixes,
            User = user,
            UserId = user.Id,
        };

        var dto = problem.ToDTO();
        
        Assert.That(dto.Id, Is.EqualTo(problem.Id));
        Assert.That(dto.Description, Is.EqualTo(problem.Description));
        Assert.That(dto.Title, Is.EqualTo(problem.Title));
        Assert.That(dto.Username, Is.EqualTo(user.UserName));
    }

    [Test]
    public void ToDTO_ThrowsException_WhenUserNull()
    {
        var fixes = new List<Fix>();
        var problem = new Problem
        {
            Id = Guid.NewGuid(),
            Description = "This is a test",
            Title = "This is a test",
            Fixes = fixes,
            User = null,
            UserId = null
        };
        
        Assert.Throws<NullReferenceException>(() => problem.ToDTO());
    }
    
}