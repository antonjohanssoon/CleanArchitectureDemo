using Application.Commands.Books.AddBook;
using Application.Dtos;
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
            var newBookDto = new BookDto
            {
                Title = "Book Title",
                Description = "Fiction"
            };
            var command = new AddBookCommand(newBookDto);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(newBookDto.Title, result.Title);
            Assert.AreEqual(newBookDto.Description, result.Description);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookTitleIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidBookDto = new BookDto
            {
                Title = "",
                Description = "Fiction"
            };
            var command = new AddBookCommand(invalidBookDto);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual("Book title is required and cannot be empty.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookDescriptionIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidBookDto = new BookDto
            {
                Title = "Book Title",
                Description = ""
            };
            var command = new AddBookCommand(invalidBookDto);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual("Book description is required and cannot be empty.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookWithSameTitleExists()
        {
            // Arrange
            var existingBook = new Book("Existing Book", "Fiction");
            var newBookDto = new BookDto
            {
                Title = "Existing Book",
                Description = "Non-Fiction"
            };
            var command = new AddBookCommand(newBookDto);

            fakeDatabase.Books.Add(existingBook);

            // Act 
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Book with title '{newBookDto.Title}' already exists.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookWithSameDescriptionExists()
        {
            // Arrange
            var existingBook = new Book("Existing Book", "Fiction");
            var newBookDto = new BookDto
            {
                Title = "New Book",
                Description = "Fiction"
            };
            var command = new AddBookCommand(newBookDto);
            fakeDatabase.Books.Add(existingBook);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Book with description '{newBookDto.Description}' already exists.", exception.Message);
        }
    }



}
