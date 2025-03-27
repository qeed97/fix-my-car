using BackendServer.Models.FixModel;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.ProblemModels.DTOs;
using BackendServer.Models.UserModels;
using BackendServer.Services.ProblemServices.Factory;

namespace BackendUnitTests.ProblemUnitTests;

[TestFixture]
public class ProblemFactoryUnitTests
{
    [Test]
    public void UpdateProblem_UpdatesExistingProblem()
    {
        var factory = new ProblemFactory();
        var problem = new Problem { Id = Guid.NewGuid(), Description = "This is a problem" , Title = "This is a problem", Fixes = new List<Fix>(), User = new User{ Id = "User1" }, UserId = "User1" };
        var updatedProblem = new UpdatedProblem{ Title = "updated title", Description = "updated description"};
        
        var newUpdatedProblem = factory.CreateNewUpdatedProblem(updatedProblem, problem);
        
        Assert.That(newUpdatedProblem.Description, Is.EqualTo("updated description"));
        Assert.That(newUpdatedProblem.Title, Is.EqualTo("updated title"));
    }
}