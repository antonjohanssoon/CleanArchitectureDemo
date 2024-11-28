using Application.Commands.Books.UpdateBook;
using Application.Dtos;
using Domain;
using Infrastructure.Database;

namespace Test.BookTests.UpdateBookTests
{
    [TestFixture]
    public class UpdateBookByIdCommandHandlerTests
    {
        private FakeDatabase fakeDatabase;
        private UpdateBookByIdCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            fakeDatabase = new FakeDatabase();
            handler = new UpdateBookByIdCommandHandler(fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldUpdateBook_WhenBookExistsAndIsValid()
        {
            // Arrange
            var existingBook = new Book("Original Title", "Original Description");
            fakeDatabase.Books.Add(existingBook);

            var updatedBookDto = new BookDto
            {
                Title = "Updated Title",
                Description = "Updated Description"
            };

            var command = new UpdateBookByIdCommand(existingBook.Id, updatedBookDto);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(updatedBookDto.Title, result.Title);
            Assert.AreEqual(updatedBookDto.Description, result.Description);
            Assert.AreEqual(existingBook.Id, result.Id);
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

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Book with ID: {nonExistentBookId} not found.", exception.Message);
        }

        [Test]
        public void ValidateUpdatedBook_ShouldThrowException_WhenBookTitleIsEmptyOrWhitespace()
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

            var existingBook = new Book("Original Title", "Original Description");
            fakeDatabase.Books.Add(existingBook);

            var command1 = new UpdateBookByIdCommand(existingBook.Id, invalidUpdatedBookDto1);
            var command2 = new UpdateBookByIdCommand(existingBook.Id, invalidUpdatedBookDto2);

            // Act
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));
            var exception2 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command2, CancellationToken.None));

            // Assert
            Assert.AreEqual("Book title cannot be empty.", exception1.Message);
            Assert.AreEqual("Book title cannot be empty.", exception2.Message);
        }

        [Test]
        public void ValidateUpdatedBook_ShouldThrowException_WhenBookDescriptionIsEmptyOrWhitespace()
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

            var existingBook = new Book("Original Title", "Original Description");
            fakeDatabase.Books.Add(existingBook);

            var command1 = new UpdateBookByIdCommand(existingBook.Id, invalidUpdatedBookDto1);
            var command2 = new UpdateBookByIdCommand(existingBook.Id, invalidUpdatedBookDto2);

            // Act
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));
            var exception2 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command2, CancellationToken.None));

            // Assert
            Assert.AreEqual("Book description cannot be empty.", exception1.Message);
            Assert.AreEqual("Book description cannot be empty.", exception2.Message);
        }
    }


}
