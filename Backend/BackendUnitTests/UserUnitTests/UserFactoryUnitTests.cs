using BackendServer.Models.UserModels.DTOs;
using BackendServer.Services.UserServices.Factory;

namespace BackendUnitTests.UserUnitTests;

[TestFixture]
public class UserFactoryUnitTests
{
    [Test]
    public void CreateUser_ValidUser_CreatesUser()
    {
        var factory = new UserFactory();
        var newUser = new NewUser{Email = "test@test.com", Password = "test123", Username = "test"};
        
        var user = factory.CreateUser(newUser);
        
        Assert.That(user.UserName, Is.EqualTo(newUser.Username));
        Assert.That(user.Email, Is.EqualTo(newUser.Email));
        Assert.That(user.Karma, Is.EqualTo(0));
    }

    [Test]
    public void CreateUser_NullUser_ThrowsException()
    {
        var factory = new UserFactory();
        Assert.Throws<NullReferenceException>(() => factory.CreateUser(null));
    }
}