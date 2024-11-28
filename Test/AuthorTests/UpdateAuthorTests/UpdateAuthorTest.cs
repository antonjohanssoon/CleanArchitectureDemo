using Application.Commands.Authors.UpdateAuthor;
using Application.Dtos;
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
            var existingAuthor = new Author("Original Name", "Original Category");
            fakeDatabase.Authors.Add(existingAuthor);

            var updatedAuthorDto = new AuthorDto
            {
                Name = "Updated Name",
                BookCategory = "Updated Category"
            };

            var command = new UpdateAuthorByIdCommand(existingAuthor.Id, updatedAuthorDto);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(updatedAuthorDto.Name, result.Name);
            Assert.AreEqual(updatedAuthorDto.BookCategory, result.BookCategory);
            Assert.AreEqual(existingAuthor.Id, result.Id);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorDoesNotExist()
        {
            // Arrange
            var updatedAuthorDto = new AuthorDto
            {
                Name = "Updated Name",
                BookCategory = "Updated Category"
            };
            var nonExistentAuthorId = Guid.NewGuid();
            var command = new UpdateAuthorByIdCommand(nonExistentAuthorId, updatedAuthorDto);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Author with ID: {nonExistentAuthorId} not found.", exception.Message);
        }

        [Test]
        public void ValidateUpdatedAuthor_ShouldThrowException_WhenAuthorNameIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidUpdatedAuthorDto1 = new AuthorDto
            {
                Name = "",
                BookCategory = "Valid Category"
            };
            var invalidUpdatedAuthorDto2 = new AuthorDto
            {
                Name = "   ",
                BookCategory = "Valid Category"
            };

            var existingAuthor = new Author("Original Name", "Original Category");
            fakeDatabase.Authors.Add(existingAuthor);

            var command1 = new UpdateAuthorByIdCommand(existingAuthor.Id, invalidUpdatedAuthorDto1);
            var command2 = new UpdateAuthorByIdCommand(existingAuthor.Id, invalidUpdatedAuthorDto2);

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
            var invalidUpdatedAuthorDto1 = new AuthorDto
            {
                Name = "Valid Name",
                BookCategory = ""
            };
            var invalidUpdatedAuthorDto2 = new AuthorDto
            {
                Name = "Valid Name",
                BookCategory = "   "
            };

            var existingAuthor = new Author("Original Name", "Original Category");
            fakeDatabase.Authors.Add(existingAuthor);

            var command1 = new UpdateAuthorByIdCommand(existingAuthor.Id, invalidUpdatedAuthorDto1);
            var command2 = new UpdateAuthorByIdCommand(existingAuthor.Id, invalidUpdatedAuthorDto2);

            // Act
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));
            var exception2 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command2, CancellationToken.None));

            // Assert
            Assert.AreEqual("Book category cannot be empty.", exception1.Message);
            Assert.AreEqual("Book category cannot be empty.", exception2.Message);
        }
    }


}
