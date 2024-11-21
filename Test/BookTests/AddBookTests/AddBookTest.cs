using Application.Commands.Books.AddBook;
using Domain;
using Infrastructure.Database;

namespace Test.BookTests.AddBookTests
{
    [TestFixture]
    public class AddBookCommandHandlerTests
    {
        private FakeDatabase fakeDatabase;
        private AddBookCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            fakeDatabase = new FakeDatabase();
            handler = new AddBookCommandHandler(fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldAddBook_WhenBookIsValid()
        {
            // Arrange
            var newBook = new Book(5, "Book Title", "Fiction");
            var command = new AddBookCommand(newBook);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(newBook, result);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookIdIsInvalid()
        {
            // Arrange
            var invalidBook = new Book(0, "Book Title", "Fiction");
            var command = new AddBookCommand(invalidBook);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            //Assert
            Assert.AreEqual("Book ID must be greater than 0 and cannot be null.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookWithSameTitleExists()
        {
            // Arrange
            var existingBook = new Book(1, "Existing Book", "Fiction");
            var newBook = new Book(1, "Existing Book", "Non-Fiction");
            var command = new AddBookCommand(newBook);

            fakeDatabase.Books.Add(existingBook);

            // Act 
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            //Assert
            Assert.AreEqual($"Book with ID '{newBook.Id}' already exists.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookWithSameIdExists()
        {
            // Arrange
            var existingBook = new Book(1, "Existing Book", "Fiction");
            var newBook = new Book(1, "New Book", "Non-Fiction");
            var command = new AddBookCommand(newBook);
            fakeDatabase.Books.Add(existingBook);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            //Assert
            Assert.AreEqual($"Book with ID '{newBook.Id}' already exists.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookTitleIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidBook1 = new Book(5, "", "Fiction");
            var command1 = new AddBookCommand(invalidBook1);

            //Act
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));

            //Assert
            Assert.AreEqual("Book title is required and cannot be empty.", exception1.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookCategoryIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidBook1 = new Book(5, "Book Title", "");
            var command1 = new AddBookCommand(invalidBook1);

            //Act
            var exception1 = Assert.ThrowsAsync<Exception>(() => handler.Handle(command1, CancellationToken.None));

            //Assert
            Assert.AreEqual("Book description is required and cannot be empty.", exception1.Message);
        }
    }

}
