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
            var book = new Book(5, "Antons self-biography", "A book about me");
            fakeDatabase.Books.Add(book);

            var query = new GetBookByIdQuery(5);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(book.Title, result.Title);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookWithIdDoesNotExist()
        {
            // Arrange
            var query = new GetBookByIdQuery(99);

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.AreEqual("Book with ID: 99 was not found.", exception.Message);
        }
    }

}
