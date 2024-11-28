using Application.Queries.Authors.GetAuthor.GetById;
using Domain;
using Infrastructure.Database;

namespace Test.AuthorTests.GetAuthorTests.GetAuthorByIdTest
{
    [TestFixture]
    public class GetAuthorByIdQueryHandlerTests
    {
        private FakeDatabase fakeDatabase;
        private GetAuthorByIdQueryHandler handler;

        [SetUp]
        public void Setup()
        {
            fakeDatabase = new FakeDatabase();
            handler = new GetAuthorByIdQueryHandler(fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldReturnAuthor_WhenAuthorExists()
        {
            // Arrange
            var author = new Author("Anton Johansson", "Sport");
            fakeDatabase.Authors.Add(author);

            var query = new GetAuthorByIdQuery(author.Id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(author.Id, result.Id);
            Assert.AreEqual(author.Name, result.Name);
            Assert.AreEqual(author.BookCategory, result.BookCategory);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorWithIdDoesNotExist()
        {
            // Arrange
            var nonExistentAuthorId = Guid.NewGuid();
            var query = new GetAuthorByIdQuery(nonExistentAuthorId);

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Author with ID: {nonExistentAuthorId} was not found.", exception.Message);
        }
    }


}
