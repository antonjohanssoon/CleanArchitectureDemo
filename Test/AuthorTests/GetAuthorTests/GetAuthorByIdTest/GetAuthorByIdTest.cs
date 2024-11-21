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
            var author = new Author(4, "Anton Johansson", "Sport");
            fakeDatabase.Authors.Add(author);

            var query = new GetAuthorByIdQuery(4);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(author, result);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorWithIdDoesNotExist()
        {
            // Arrange
            var query = new GetAuthorByIdQuery(99);

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.AreEqual("Author with ID: 99 was not found.", exception.Message);
        }
    }

}
