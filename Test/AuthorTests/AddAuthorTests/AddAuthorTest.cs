using Application.Commands.Authors.AddAuthor;
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
            var newAuthor = new Author
            {
                Name = "Author Name",
                BookCategory = "Fiction"
            };
            var command = new AddAuthorCommand(newAuthor);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(newAuthor.Name, result.Name);
            Assert.AreEqual(newAuthor.BookCategory, result.BookCategory);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorNameIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidAuthor = new Author
            {
                Name = "",
                BookCategory = "Fiction"
            };
            var command = new AddAuthorCommand(invalidAuthor);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual("Author name is required and cannot be empty.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorBookCategoryIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidAuthor = new Author
            {
                Name = "Author Name",
                BookCategory = ""
            };
            var command = new AddAuthorCommand(invalidAuthor);

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
            var newAuthor = new Author
            {
                Name = "Existing Author",
                BookCategory = "Non-Fiction"
            };
            var command = new AddAuthorCommand(newAuthor);

            fakeDatabase.Authors.Add(existingAuthor);

            // Act 
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Author with name '{newAuthor.Name}' already exists.", exception.Message);
        }

    }

}
