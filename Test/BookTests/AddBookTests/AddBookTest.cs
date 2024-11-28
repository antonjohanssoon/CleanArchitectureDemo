using Application.Commands.Books.AddBook;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Moq;

namespace Test.BookTests.AddBookTests
{
    [TestFixture]
    public class AddBookCommandHandlerTests
    {
        private Mock<IRepository<Book>> mockBookRepository;
        private AddBookCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            mockBookRepository = new Mock<IRepository<Book>>();
            handler = new AddBookCommandHandler(mockBookRepository.Object);
        }

        [Test]
        public async Task Handle_ShouldAddBook_WhenBookIsValid()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Book Title",
                Description = "Fiction"
            };
            var command = new AddBookCommand(newBook);

            mockBookRepository.Setup(repo => repo.Add(It.IsAny<Book>())).Verifiable();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mockBookRepository.Verify(repo => repo.Add(It.IsAny<Book>()), Times.Once);
            Assert.AreEqual(newBook.Title, result.Title);
            Assert.AreEqual(newBook.Description, result.Description);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookTitleIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidBook = new Book
            {
                Title = "",
                Description = "Fiction"
            };
            var command = new AddBookCommand(invalidBook);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual("Book title is required and cannot be empty.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookDescriptionIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidBook = new Book
            {
                Title = "Book Title",
                Description = ""
            };
            var command = new AddBookCommand(invalidBook);

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
            var newBook = new Book
            {
                Title = "Existing Book",
                Description = "Non-Fiction"
            };
            var command = new AddBookCommand(newBook);

            mockBookRepository.Setup(repo => repo.GetAll()).Returns(new[] { existingBook });

            // Act 
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Book with title '{newBook.Title}' already exists.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookWithSameDescriptionExists()
        {
            // Arrange
            var existingBook = new Book("Existing Book", "Fiction");
            var newBook = new Book
            {
                Title = "New Book",
                Description = "Fiction"
            };
            var command = new AddBookCommand(newBook);

            mockBookRepository.Setup(repo => repo.GetAll()).Returns(new[] { existingBook });

            // Act 
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Book with description '{newBook.Description}' already exists.", exception.Message);
        }
    }
}

