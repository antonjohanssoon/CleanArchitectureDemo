using Application.Commands.Books.UpdateBook;
using Application.Dtos;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Moq;

namespace Test.BookTests.UpdateBookTests
{
    [TestFixture]
    public class UpdateBookByIdCommandHandlerTests
    {
        private Mock<IRepository<Book>> mockBookRepository;
        private UpdateBookByIdCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            mockBookRepository = new Mock<IRepository<Book>>();
            handler = new UpdateBookByIdCommandHandler(mockBookRepository.Object);
        }

        [Test]
        public async Task Handle_ShouldUpdateBook_WhenBookExistsAndIsValid()
        {
            // Arrange
            var existingBook = new Book("Original Title", "Original Description");
            var updatedBookDto = new BookDto
            {
                Title = "Updated Title",
                Description = "Updated Description"
            };
            var command = new UpdateBookByIdCommand(existingBook.Id, updatedBookDto);

            mockBookRepository.Setup(repo => repo.GetById(existingBook.Id)).Returns(existingBook);
            mockBookRepository.Setup(repo => repo.Update(It.IsAny<Book>())).Verifiable();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mockBookRepository.Verify(repo => repo.Update(It.IsAny<Book>()), Times.Once);
            Assert.AreEqual(updatedBookDto.Title, result.Data.Title);
            Assert.AreEqual(updatedBookDto.Description, result.Data.Description);
            Assert.AreEqual(existingBook.Id, result.Data.Id);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookDoesNotExist()
        {
            // Arrange
            var updatedBookDto = new BookDto
            {
                Title = "Updated Title",
                Description = "Updated Description"
            };
            var nonExistentBookId = Guid.NewGuid();
            var command = new UpdateBookByIdCommand(nonExistentBookId, updatedBookDto);

            mockBookRepository.Setup(repo => repo.GetById(nonExistentBookId)).Returns((Book)null);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Book with ID: {nonExistentBookId} not found.", exception.Message);
        }

        [Test]
        public async Task ValidateUpdatedBook_ShouldThrowException_WhenBookTitleIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidUpdatedBookDto1 = new BookDto
            {
                Title = "",
                Description = "Valid Description"
            };
            var invalidUpdatedBookDto2 = new BookDto
            {
                Title = "   ",
                Description = "Valid Description"
            };

            // Mocka en bok som ska uppdateras
            var existingBook = new Book("Original Title", "Original Description");
            mockBookRepository.Setup(repo => repo.GetById(existingBook.Id)).Returns(existingBook);

            var command1 = new UpdateBookByIdCommand(existingBook.Id, invalidUpdatedBookDto1);
            var command2 = new UpdateBookByIdCommand(existingBook.Id, invalidUpdatedBookDto2);

            // Act & Assert
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));
            var exception2 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command2, CancellationToken.None));

            Assert.AreEqual("Book title cannot be empty.", exception1.Message);
            Assert.AreEqual("Book title cannot be empty.", exception2.Message);
        }

        [Test]
        public async Task ValidateUpdatedBook_ShouldThrowException_WhenBookDescriptionIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidUpdatedBookDto1 = new BookDto
            {
                Title = "Valid Title",
                Description = ""
            };
            var invalidUpdatedBookDto2 = new BookDto
            {
                Title = "Valid Title",
                Description = "   "
            };

            // Mocka en bok som ska uppdateras
            var existingBook = new Book("Original Title", "Original Description");
            mockBookRepository.Setup(repo => repo.GetById(existingBook.Id)).Returns(existingBook);

            var command1 = new UpdateBookByIdCommand(existingBook.Id, invalidUpdatedBookDto1);
            var command2 = new UpdateBookByIdCommand(existingBook.Id, invalidUpdatedBookDto2);

            // Act & Assert
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));
            var exception2 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command2, CancellationToken.None));

            Assert.AreEqual("Book description cannot be empty.", exception1.Message);
            Assert.AreEqual("Book description cannot be empty.", exception2.Message);
        }
    }
}

