using BackendServer.Models.FixModel;
using BackendServer.Models.FixModel.DTOs;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.UserModels;
using BackendServer.Services.FixServices.Factory;
using Moq;

namespace BackendUnitTests.FixUnitTests;

[TestFixture]
public class FixFactoryUnitTests
{
    [Test]
    public void CreateFix_CreatesNewFix()
    {
        var factory = new FixFactory();
        var newfix = new NewFix { Content = "New fix content" };
        var user = new User { Id = "userId" };
        var problem = new Problem { Id = Guid.NewGuid() };
        
        var fix = factory.CreateFix(newfix, problem, user);

        Assert.That(fix.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(fix.Content, Is.EqualTo(newfix.Content));
        Assert.That(fix.Problem, Is.EqualTo(problem));
        Assert.That(fix.User, Is.EqualTo(user));
    }

    [Test]
    public void CreateFix_NullProblem_ThrowsException()
    {
        var factory = new FixFactory();
        var user = new User { Id = "userId" };
        var newfix = new NewFix { Content = "New fix content" };
        
        Assert.Throws<NullReferenceException>(() => factory.CreateFix(newfix, null, user));
    }

    [Test]
    public void CreateFix_NullUser_ThrowsException()
    {
        var factory = new FixFactory();
        var newfix = new NewFix { Content = "New fix content" };
        var problem = new Problem { Id = Guid.NewGuid() };
        
        Assert.Throws<NullReferenceException>(() => factory.CreateFix(newfix, problem, null));
    }

    [Test]
    public void UpdateFix_UpdatesExistingFix()
    {
        var factory = new FixFactory();
        var fix = new Fix { Content = "New fix content" };
        var newContent = "Updated fix content";
        
        var updatedFix = factory.UpdateFix(newContent, fix);
        
        Assert.That(updatedFix.Content, Is.EqualTo(newContent));
    }

    [Test]
    public void UpdateFix_ThrowsException()
    {
        var factory = new FixFactory();
        var newContent = "Updated fix content";
        
        Assert.Throws<NullReferenceException>(() => factory.UpdateFix(newContent, null));
    }
}