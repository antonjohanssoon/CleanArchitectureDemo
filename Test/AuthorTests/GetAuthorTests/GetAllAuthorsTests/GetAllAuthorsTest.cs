using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Authors.GetAuthor.GetAll;
using Domain;
using Moq;

namespace Test.AuthorTests.GetAuthorTests.GetAllAuthorsTests
{
    [TestFixture]
    public class GetAllAuthorsTest
    {
        private Mock<IRepository<Author>> authorRepositoryMock;
        private GetAllAuthorsFromDBQueryHandler handler;

        [SetUp]
        public void Setup()
        {
            authorRepositoryMock = new Mock<IRepository<Author>>();
            handler = new GetAllAuthorsFromDBQueryHandler(authorRepositoryMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnAuthors_WhenAuthorsExist()
        {
            // Arrange
            var newAuthor = new Author("Anton Johansson", "Sport");

            authorRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Author> { newAuthor });

            var query = new GetAllAuthorsFromDBQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(1, result.Data.Count);
            Assert.Contains(newAuthor, result.Data);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorsListIsEmpty()
        {
            // Arrange
            authorRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Author>());

            var query = new GetAllAuthorsFromDBQuery();

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.AreEqual("Your list of authors is empty", exception.Message);
        }
    }
}

