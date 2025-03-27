using BackendServer.Extensions;
using BackendServer.Models.FixModel;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.UserModels;

namespace BackendUnitTests.UserUnitTests;

[TestFixture]
public class UserExtensionsUnitTests
{
    [Test]
    public void ToDTO_ConvertsCorrectly()
    {
        var user = new User{ UserName = "user1", Problems = new List<Problem>(), Fixes = new List<Fix>(), Karma = 1, SessionToken = "ddsadsad"};

        var dto = user.ToDTO();
        
        Assert.That(dto.Username, Is.EqualTo(user.UserName));
        Assert.That(dto.SessionToken, Is.EqualTo(user.SessionToken));
        Assert.That(dto.karma, Is.EqualTo(user.Karma));
    }

    [Test]
    public void ToDTO_Null_ThrowsException()
    {
        var user = new User{UserName = "user1", Problems = null, Fixes = null, Karma = 1, SessionToken = "ddsadsad"};
        
        Assert.Throws<ArgumentNullException>(() => user.ToDTO());
    }
}