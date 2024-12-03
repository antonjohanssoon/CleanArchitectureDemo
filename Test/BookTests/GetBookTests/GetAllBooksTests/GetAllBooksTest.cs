using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Books.GetBook.GetAll;
using Domain;
using Moq;

namespace Test.BookTests.GetBookTests.GetAllBooksTests
{
    [TestFixture]
    public class GetBookTest
    {
        private Mock<IRepository<Book>> mockBookRepository;
        private GetAllBooksFromDBQueryHandler handler;

        [SetUp]
        public void Setup()
        {
            mockBookRepository = new Mock<IRepository<Book>>();
            handler = new GetAllBooksFromDBQueryHandler(mockBookRepository.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnBooks_WhenBooksExist()
        {
            // Arrange
            var newBook = new Book("The Great Adventure", "Fiction");
            var books = new List<Book> { newBook };
            var query = new GetAllBooksFromDBQuery();

            mockBookRepository.Setup(repo => repo.GetAll()).Returns(books);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(1, result.Data.Count);
            Assert.Contains(newBook, result.Data);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBooksListIsEmpty()
        {
            // Arrange
            var books = new List<Book>();
            var query = new GetAllBooksFromDBQuery();

            mockBookRepository.Setup(repo => repo.GetAll()).Returns(books);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.AreEqual("Your list of books is empty", exception.Message);
        }
    }
}

