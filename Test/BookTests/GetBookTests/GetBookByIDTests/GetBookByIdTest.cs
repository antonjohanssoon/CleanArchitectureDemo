using Application.Queries.Books.GetBook.GetById;
using Domain;
using Infrastructure.Database;

namespace Test.BookTests.GetBookTests.GetBookByIDTests
{
    [TestFixture]
    public class GetBookByIdQueryHandlerTests
    {
        private FakeDatabase fakeDatabase;
        private GetBookByIdQueryHandler handler;

        [SetUp]
        public void Setup()
        {
            fakeDatabase = new FakeDatabase();
            handler = new GetBookByIdQueryHandler(fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var book = new Book("Antons self-biography", "A book about me");
            fakeDatabase.Books.Add(book);

            var query = new GetBookByIdQuery(book.Id);

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

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Book with ID: {nonExistentBookId} was not found.", exception.Message);
        }
    }


}
