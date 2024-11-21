using Application.Commands.Books.UpdateBook;
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
            var existingBook = new Book(5, "Original Title", "Original Description");
            fakeDatabase.Books.Add(existingBook);

            var updatedBook = new Book(5, "Updated Title", "Updated Description");
            var command = new UpdateBookByIdCommand(5, updatedBook);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(updatedBook.Title, result.Title);
            Assert.AreEqual(updatedBook.Description, result.Description);
            Assert.AreEqual(updatedBook.Id, result.Id);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookDoesNotExist()
        {
            // Arrange
            var updatedBook = new Book(99, "Updated Title", "Updated Description");
            var command = new UpdateBookByIdCommand(99, updatedBook);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual("Book with ID: 99 not found.", exception.Message);
        }
        [Test]
        public void ValidateUpdatedBook_ShouldThrowException_WhenBookTitleIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidUpdatedBook1 = new Book(19, "", "Valid Description");
            var invalidUpdatedBook2 = new Book(19, "   ", "Valid Description");

            var command1 = new UpdateBookByIdCommand(19, invalidUpdatedBook1);
            var command2 = new UpdateBookByIdCommand(19, invalidUpdatedBook2);

            fakeDatabase.Books.Add(new Book(19, "Original Title", "Original Description"));

            // Act
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));
            var exception2 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command2, CancellationToken.None));

            // Assert
            Assert.AreEqual("Book title cannot be empty.", exception2.Message);
            Assert.AreEqual("Book title cannot be empty.", exception1.Message);
        }

        [Test]
        public void ValidateUpdatedBook_ShouldThrowException_WhenBookDescriptionIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidUpdatedBook1 = new Book(18, "Valid Title", "");
            var invalidUpdatedBook2 = new Book(18, "Valid Title", "   ");

            var command1 = new UpdateBookByIdCommand(18, invalidUpdatedBook1);
            var command2 = new UpdateBookByIdCommand(18, invalidUpdatedBook2);

            fakeDatabase.Books.Add(new Book(18, "Original Title", "Original Description"));

            // Act
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));
            var exception2 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command2, CancellationToken.None));

            // Assert
            Assert.AreEqual("Book description cannot be empty.", exception2.Message);
            Assert.AreEqual("Book description cannot be empty.", exception1.Message);
        }
    }

}
