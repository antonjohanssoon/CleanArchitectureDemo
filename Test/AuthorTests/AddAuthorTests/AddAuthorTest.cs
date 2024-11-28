using Application.Commands.Authors.AddAuthor;
using Application.Dtos;
using Domain;
using Infrastructure.Database;

namespace Test.AuthorTests.AddAuthorTests
{
    [TestFixture]
    public class AddAuthorCommandHandlerTests
    {
        private FakeDatabase fakeDatabase;
        private AddAuthorCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            fakeDatabase = new FakeDatabase();
            handler = new AddAuthorCommandHandler(fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldAddAuthor_WhenAuthorIsValid()
        {
            // Arrange
            var newAuthorDto = new AuthorDto
            {
                Name = "Author Name",
                BookCategory = "Fiction"
            };
            var command = new AddAuthorCommand(newAuthorDto);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(newAuthorDto.Name, result.Name);
            Assert.AreEqual(newAuthorDto.BookCategory, result.BookCategory);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorNameIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidAuthorDto = new AuthorDto
            {
                Name = "",
                BookCategory = "Fiction"
            };
            var command = new AddAuthorCommand(invalidAuthorDto);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual("Author name is required and cannot be empty.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorBookCategoryIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidAuthorDto = new AuthorDto
            {
                Name = "Author Name",
                BookCategory = ""
            };
            var command = new AddAuthorCommand(invalidAuthorDto);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual("Author book category is required and cannot be empty.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorWithSameNameExists()
        {
            // Arrange
            var existingAuthor = new Author("Existing Author", "Fiction");
            var newAuthorDto = new AuthorDto
            {
                Name = "Existing Author",
                BookCategory = "Non-Fiction"
            };
            var command = new AddAuthorCommand(newAuthorDto);

            fakeDatabase.Authors.Add(existingAuthor);

            // Act 
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Author with name '{newAuthorDto.Name}' already exists.", exception.Message);
        }

    }

}
