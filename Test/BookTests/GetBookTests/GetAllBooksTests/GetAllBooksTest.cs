using Application.Queries.Books.GetBook.GetAll;
using Domain;
using Infrastructure.Database;

namespace Test.BookTests.GetBookTests.GetAllBooksTests
{
    [TestFixture]
    public class GetBookTest
    {
        private FakeDatabase fakeDatabase;
        private GetAllBooksFromDBQueryHandler handler;

        [SetUp]
        public void Setup()
        {
            fakeDatabase = new FakeDatabase();
            handler = new GetAllBooksFromDBQueryHandler(fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldReturnBooks_WhenBooksExist()
        {
            // Arrange
            var newBook = new Book(4, "The Great Adventure", "Fiction");
            fakeDatabase.Books.Add(newBook);

            var query = new GetAllBooksFromDBQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(4, result.Count);
            Assert.Contains(newBook, result);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBooksListIsEmpty()
        {
            // Arrange
            fakeDatabase.Books.Clear();
            var query = new GetAllBooksFromDBQuery();

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.AreEqual("Your list of books is empty", exception.Message);
        }
    }

}
