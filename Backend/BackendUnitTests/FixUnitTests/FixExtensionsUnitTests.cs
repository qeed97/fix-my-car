using BackendServer.Extensions;
using BackendServer.Models.FixModel;
using BackendServer.Models.UserModels;
using Moq;

namespace BackendUnitTests.FixUnitTests;

[TestFixture]
public class FixExtensionsUnitTests
{
    [Test]
    public void ToDTO_ConvertsCorrectly()
    {
        var mockUser = new Mock<User>();
        mockUser.Setup(u => u.UserName).Returns("MockUser");

        var fix = new Fix
        {
            Id = Guid.NewGuid(),
            Content = "Test fix",
            Fixed = false,
            User = mockUser.Object
        };

        var dto = fix.ToDTO();
        
        Assert.That(dto.Id, Is.EqualTo(fix.Id));
        Assert.That(dto.Content, Is.EqualTo(fix.Content));
        Assert.That(dto.Fixed, Is.EqualTo(fix.Fixed));
        Assert.That(dto.Username, Is.EqualTo(mockUser.Object.UserName));
    }

    [Test]
    public void ToDTO_NullUser_ThrowsException()
    {
        var fix = new Fix
        {
            Id = Guid.NewGuid(),
            Content = "Test fix",
            Fixed = false,
            User = null
        };
        
        Assert.Throws<NullReferenceException>(() => fix.ToDTO());
    }
}