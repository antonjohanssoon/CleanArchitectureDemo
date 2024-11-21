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
            var newAuthor = new Author(5, "Author Name", "Fiction");
            var command = new AddAuthorCommand(newAuthor);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(newAuthor, result);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorIdIsInvalid()
        {
            // Arrange
            var invalidAuthor = new Author(0, "Author Name", "Fiction");
            var command = new AddAuthorCommand(invalidAuthor);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            //Assert
            Assert.AreEqual("Author ID must be greater than 0 and cannot be null.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorWithSameNameExists()
        {
            // Arrange
            var existingAuthor = new Author(1, "Existing Author", "Fiction");
            var newAuthor = new Author(1, "Existing Author", "Non-Fiction");
            var command = new AddAuthorCommand(newAuthor);

            fakeDatabase.Authors.Add(existingAuthor);

            // Act 
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            //Assert
            Assert.AreEqual($"Author with name '{newAuthor.Name}' already exists.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorWithSameIdExists()
        {
            // Arrange
            var existingAuthor = new Author(1, "Existing Author", "Fiction");
            var newAuthor = new Author(1, "New Author", "Non-Fiction");
            var command = new AddAuthorCommand(newAuthor);
            fakeDatabase.Authors.Add(existingAuthor);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            //Assert
            Assert.AreEqual($"Author with ID '{newAuthor.Id}' already exists.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorNameIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidAuthor1 = new Author(5, "", "Fiction");
            var command1 = new AddAuthorCommand(invalidAuthor1);

            //Act
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));

            //Assert
            Assert.AreEqual("Author name is required and cannot be empty.", exception1.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorBookCategoryIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidAuthor1 = new Author(5, "Author Name", "");
            var command1 = new AddAuthorCommand(invalidAuthor1);

            //Act
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));

            //Assert
            Assert.AreEqual("Author book category is required and cannot be empty.", exception1.Message);
        }
    }
}
