using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Authors.GetAuthor.GetById;
using Domain;
using Moq;

namespace Test.AuthorTests.GetAuthorTests.GetAuthorByIdTest
{
    [TestFixture]
    public class GetAuthorByIdQueryHandlerTests
    {
        private Mock<IRepository<Author>> authorRepositoryMock;
        private GetAuthorByIdQueryHandler handler;

        [SetUp]
        public void Setup()
        {
            // Mocka IRepository<Author>
            authorRepositoryMock = new Mock<IRepository<Author>>();

            // Skapa handler och injicera mocken
            handler = new GetAuthorByIdQueryHandler(authorRepositoryMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnAuthor_WhenAuthorExists()
        {
            // Arrange
            var author = new Author("Anton Johansson", "Sport");

            // Mocka GetById för att returnera en författare direkt
            authorRepositoryMock.Setup(repo => repo.GetById(author.Id)).Returns(author);

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

            // Mocka GetById för att returnera null för icke-existerande författare
            authorRepositoryMock.Setup(repo => repo.GetById(nonExistentAuthorId)).Returns((Author)null);

            var query = new GetAuthorByIdQuery(nonExistentAuthorId);

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Author with ID: {nonExistentAuthorId} was not found.", exception.Message);
        }
    }

}

