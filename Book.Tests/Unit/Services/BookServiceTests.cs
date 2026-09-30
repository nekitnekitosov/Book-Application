using Book.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace Book.Book.Tests.Unit.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _mockRepo;
    private readonly BookService _sut;

    public BookServiceTests()
    {
        _mockRepo = new Mock<IBookRepository>();
        _sut = new BookService(_mockRepo.Object);
    }
    [Fact]
    public async Task AddBookAsync_Should_Throw_ConfclictException_When_BookExists()
    {
        _mockRepo
            .Setup(r => r.FindBookAsync("1984"))
            .ReturnsAsync("1984");

        var request = new BookRequest
        {
            BookName = "1984",
            AuthorName = "Джордж Оруэлл",
            YearOfPublish = "1949",
            Description = "Роман-антиутопия"
        };

        var role = "Admin";
        var userId = 1;

        Func<Task> act = async () => await _sut.AddBook(request, role, userId);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("Такая книга уже существует в базе!");
    }
}
