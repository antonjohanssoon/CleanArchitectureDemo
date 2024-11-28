using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Books.GetBook.GetById;
using Domain;
using Moq;

namespace Test.BookTests.GetBookTests.GetBookByIDTests
{
    [TestFixture]
    public class GetBookByIdQueryHandlerTests
    {
        private Mock<IRepository<Book>> mockBookRepository;
        private GetBookByIdQueryHandler handler;

        [SetUp]
        public void Setup()
        {
            // Mocka IRepository<Book>
            mockBookRepository = new Mock<IRepository<Book>>();
            handler = new GetBookByIdQueryHandler(mockBookRepository.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var book = new Book("Antons self-biography", "A book about me");
            var query = new GetBookByIdQuery(book.Id);

            // Mocka att GetById() returnerar boken
            mockBookRepository.Setup(repo => repo.GetById(book.Id)).Returns(book);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(book.Id, result.Id);
            Assert.AreEqual(book.Title, result.Title);
            Assert.AreEqual(book.Description, result.Description);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookWithIdDoesNotExist()
        {
            // Arrange
            var nonExistentBookId = Guid.NewGuid();
            var query = new GetBookByIdQuery(nonExistentBookId);

            // Mocka att GetById() returnerar null för en ogiltig bok-ID
            mockBookRepository.Setup(repo => repo.GetById(nonExistentBookId)).Returns((Book)null);

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Book with ID: {nonExistentBookId} was not found.", exception.Message);
        }
    }
}

