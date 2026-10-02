using Book.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.EventHandlers;
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
    [Fact] // тест на успешную регистрацию
    public async Task Login_Should_ReturnTokens_When_CredentialsAreValid()
    {
        _mockRepoUser
            .Setup(r => r.FindUser("1"))
            .ReturnsAsync(true);

        _mockRepoUser
            .Setup(r => r.LoginUserAsync(It.IsAny<UserLoginRequest>()))
            .ReturnsAsync(new User
            {
                UserId = 1,
                UserName = "1",
                Role = "User"
            });

        _mockServiceToken
            .Setup(r => r.GenerateJwtToken(It.IsAny<User>()))
            .Returns("access_token");
        _mockServiceToken
            .Setup(r => r.GenerateRefreshToken())
            .Returns("refresh_token");

        _mockRepoToken
            .Setup(r => r.AddRefreshTokenAsync(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        var user = new UserLoginRequest
        {
            Username = "1",
            Password = "12345678"
        };

        var result = await _userService.LoginUser(user);

        result.Should().NotBeNull(); // метод вернул не null
        result.AccessToken.Should().Be("access_token"); // вернул правильный токен
        result.RefreshToken.Should().Be("refresh_token"); // вернул правильный токен

        _mockServiceToken.Verify(t => t.GenerateJwtToken(It.IsAny<User>()), Times.Once); // проверка, что вызван один раз
        _mockServiceToken.Verify(t => t.GenerateRefreshToken(), Times.Once); // проверка, что вызван один раз

        _mockRepoToken.Verify(r => r.AddRefreshTokenAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once); // проверка, что вызван один раз
        _mockRepoUser.Verify(r => r.FindUser("1"), Times.Once); // найден пользователь
        _mockRepoUser.Verify(r => r.LoginUserAsync(It.IsAny<UserLoginRequest>()), Times.Once);
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

    [Theory] // тест на валидацию данных в запросе при регистрации
    [InlineData(null, "12345678")] // null имя
    [InlineData("", "12345678")] // пустое имя
    [InlineData("   ", "12345678")] // пробелы в username
    [InlineData("Tom", "123")] // короткий пароль
    [InlineData("Tom", "")] // пустой пароль
    public async Task Register_Should_ThrowValidationException_When_InputIsInvalid(string username, string password)
    {
        _mockRepoUser
            .Setup(u => u.FindUser(It.IsAny<string>()))
            .ReturnsAsync(false);

        var newUser = new UserRequest
        {
            UserName = username,
            Password = password
        };

        Func<Task> act = async () => await _userService.RegisterUser(newUser);

        await act.Should().ThrowAsync<ValidationException>();
    }
    [Fact] // тест на успешную регистрацию
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
    [Fact] // неверный пароль при авторизации
    public async Task Login_Should_ThrowUnauthorizedException_When_PasswordIsWrong()
    {
        _mockRepoUser
            .Setup(r => r.FindUser("1"))
            .ReturnsAsync(true);

        _mockRepoUser
            .Setup(t => t.LoginUserAsync(It.IsAny<UserLoginRequest>()))
            .ThrowsAsync(new UnauthorizedException("Неправильный пароль"));

        var user = new UserLoginRequest
        {
            Username = "1",
            Password = "12345678"
        };

        Func<Task> act = async () => await _userService.LoginUser(user);

        await act.Should().ThrowAsync<UnauthorizedException>();

        _mockServiceToken.Verify(t => t.GenerateJwtToken(It.IsAny<User>()), Times.Never);
        _mockRepoToken.Verify(t => t.AddRefreshTokenAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
    }
    [Fact] // тест на несуществующего user
    public async Task Login_Should_ThrowNotFoundException_When_UserNotFound()
    {
        _mockRepoUser
            .Setup(r => r.FindUser("1"))
            .ReturnsAsync(false);
        var user = new UserLoginRequest
        {
            Username = "1",
            Password = "12345678"
        };

        Func<Task> act = async () => await _userService.LoginUser(user);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
