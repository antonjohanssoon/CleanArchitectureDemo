using Application.Commands.Authors.UpdateAuthor;
using Domain;
using Infrastructure.Database;

namespace Test.AuthorTests.UpdateAuthorTests
{
    [TestFixture]
    public class UpdateAuthorByIdCommandHandlerTests
    {
        private FakeDatabase fakeDatabase;
        private UpdateAuthorByIdCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            fakeDatabase = new FakeDatabase();
            handler = new UpdateAuthorByIdCommandHandler(fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldUpdateAuthor_WhenAuthorExistsAndIsValid()
        {
            // Arrange
            var existingAuthor = new Author(5, "Original Name", "Original Category");
            fakeDatabase.Authors.Add(existingAuthor);

            var updatedAuthor = new Author(5, "Updated Name", "Updated Category");
            var command = new UpdateAuthorByIdCommand(5, updatedAuthor);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(updatedAuthor.Name, result.Name);
            Assert.AreEqual(updatedAuthor.BookCategory, result.BookCategory);
            Assert.AreEqual(updatedAuthor.Id, result.Id);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorDoesNotExist()
        {
            // Arrange
            var updatedAuthor = new Author(99, "Updated Name", "Updated Category");
            var command = new UpdateAuthorByIdCommand(99, updatedAuthor);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual("Author with ID: 99 not found.", exception.Message);
        }

        [Test]
        public void ValidateUpdatedAuthor_ShouldThrowException_WhenAuthorNameIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidUpdatedAuthor1 = new Author(19, "", "Valid Category");
            var invalidUpdatedAuthor2 = new Author(19, "   ", "Valid Category");

            var command1 = new UpdateAuthorByIdCommand(19, invalidUpdatedAuthor1);
            var command2 = new UpdateAuthorByIdCommand(19, invalidUpdatedAuthor2);

            fakeDatabase.Authors.Add(new Author(19, "Original Name", "Original Category"));

            // Act
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));
            var exception2 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command2, CancellationToken.None));

            // Assert
            Assert.AreEqual("Author name cannot be empty.", exception1.Message);
            Assert.AreEqual("Author name cannot be empty.", exception2.Message);
        }

        [Test]
        public void ValidateUpdatedAuthor_ShouldThrowException_WhenAuthorBookCategoryIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidUpdatedAuthor1 = new Author(18, "Valid Name", "");
            var invalidUpdatedAuthor2 = new Author(18, "Valid Name", "   ");

            var command1 = new UpdateAuthorByIdCommand(18, invalidUpdatedAuthor1);
            var command2 = new UpdateAuthorByIdCommand(18, invalidUpdatedAuthor2);

            fakeDatabase.Authors.Add(new Author(18, "Original Name", "Original Category"));

            // Act
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));
            var exception2 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command2, CancellationToken.None));

            // Assert
            Assert.AreEqual("Book category cannot be empty.", exception1.Message);
            Assert.AreEqual("Book category cannot be empty.", exception2.Message);
        }
    }

}
