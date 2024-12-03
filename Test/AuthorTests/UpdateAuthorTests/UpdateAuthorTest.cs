using Application.Commands.Authors.UpdateAuthor;
using Application.Dtos;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Moq;

namespace Test.AuthorTests.UpdateAuthorTests
{
    [TestFixture]
    public class UpdateAuthorByIdCommandHandlerTests
    {
        private Mock<IRepository<Author>> authorRepositoryMock;
        private UpdateAuthorByIdCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            authorRepositoryMock = new Mock<IRepository<Author>>();
            handler = new UpdateAuthorByIdCommandHandler(authorRepositoryMock.Object);
        }

        [Test]
        public void Handle_ShouldUpdateAuthor_WhenAuthorExistsAndIsValid()
        {
            // Arrange
            var existingAuthor = new Author("Original Name", "Original Category");

            authorRepositoryMock.Setup(repo => repo.GetById(existingAuthor.Id)).Returns(existingAuthor);

            var updatedAuthorDto = new AuthorDto
            {
                Name = "Updated Name",
                BookCategory = "Updated Category"
            };

            var command = new UpdateAuthorByIdCommand(existingAuthor.Id, updatedAuthorDto);

            // Act
            var result = handler.Handle(command, CancellationToken.None).Result;

            // Assert
            Assert.AreEqual(updatedAuthorDto.Name, result.Data.Name);
            Assert.AreEqual(updatedAuthorDto.BookCategory, result.Data.BookCategory);
            Assert.AreEqual(existingAuthor.Id, result.Data.Id);
        }

        [Test]
        public async Task Handle_ShouldThrowException_WhenAuthorDoesNotExist()
        {
            // Arrange
            var updatedAuthorDto = new AuthorDto
            {
                Name = "Updated Name",
                BookCategory = "Updated Category"
            };
            var nonExistentAuthorId = Guid.NewGuid();

            authorRepositoryMock.Setup(repo => repo.GetById(nonExistentAuthorId)).Returns((Author)null);


            var command = new UpdateAuthorByIdCommand(nonExistentAuthorId, updatedAuthorDto);

            // Act and Assert
            var exception = Assert.Throws<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Author with ID {nonExistentAuthorId} not found.", exception.Message);
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

            authorRepositoryMock.Setup(repo => repo.GetById(existingAuthor.Id)).Returns(existingAuthor);

            var command1 = new UpdateAuthorByIdCommand(existingAuthor.Id, invalidUpdatedAuthorDto1);
            var command2 = new UpdateAuthorByIdCommand(existingAuthor.Id, invalidUpdatedAuthorDto2);

            // Act & Assert
            var exception1 = Assert.Throws<Exception>(() => handler.Handle(command1, CancellationToken.None));
            var exception2 = Assert.Throws<Exception>(() => handler.Handle(command2, CancellationToken.None));

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

            authorRepositoryMock.Setup(repo => repo.GetById(existingAuthor.Id)).Returns(existingAuthor);

            var command1 = new UpdateAuthorByIdCommand(existingAuthor.Id, invalidUpdatedAuthorDto1);
            var command2 = new UpdateAuthorByIdCommand(existingAuthor.Id, invalidUpdatedAuthorDto2);

            // Act & Assert
            var exception1 = Assert.Throws<Exception>(() => handler.Handle(command1, CancellationToken.None));
            var exception2 = Assert.Throws<Exception>(() => handler.Handle(command2, CancellationToken.None));

            Assert.AreEqual("Book category cannot be empty.", exception1.Message);
            Assert.AreEqual("Book category cannot be empty.", exception2.Message);
        }
    }


}

