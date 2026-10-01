using Book.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity.Data;
using Moq;
using Xunit;

namespace Book.Tests.Unit.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepoUser;
    private readonly Mock<ITokenService> _mockServiceToken;
    private readonly Mock<ITokenRepository> _mockRepoToken;

    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockRepoUser = new Mock<IUserRepository>();
        _mockServiceToken = new Mock<ITokenService>();
        _mockRepoToken = new Mock<ITokenRepository>();

        _userService = new UserService(_mockRepoUser.Object, _mockServiceToken.Object, _mockRepoToken.Object);
    }
    [Fact]
    public async Task Register_Should_Throw_Conflict_When_UsernameExists()
    {
        _mockRepoUser
            .Setup(r => r.FindUser("123"))
            .ReturnsAsync(true);

        var newUser = new UserRequest
        {
            RoleId = 1,
            UserName = "123",
            Password = "12345678"
        };

        Func<Task> act = async () => await _userService.RegisterUser(newUser);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("С таким именем пользователь уже существует");
    }
    [Fact]
    public async Task Register_Should_CreateUser_When_UsernameIsFree()
    {
        _mockRepoUser
            .Setup(r => r.FindUser("123"))
            .ReturnsAsync(false);

        _mockRepoUser
            .Setup(r => r.RegisterUserAsync(It.IsAny<UserRequest>()))
            .ReturnsAsync((UserRequest req) => new User
            {
                UserName = req.UserName,
            });

        _mockServiceToken
            .Setup(t => t.GenerateJwtToken(It.IsAny<User>()))
            .Returns("fake_jwt_token");

        var newUser = new UserRequest
        {
            RoleId = 1,
            UserName = "123",
            Password = "12345678"
        };

        var result = await _userService.RegisterUser(newUser);

        result.Should().NotBeNull(); // проверка, что RegisterUser вернул объект User
        result.UserName.Should().Be("123"); // Проверка, что имена совпадают с запросом

        _mockRepoUser.Verify(r => r.RegisterUserAsync(newUser), Times.Once); // проверяем, что метод вызвался ровно один раз, times.once - гарантирует, что пользователь создан один раз
    }
}
