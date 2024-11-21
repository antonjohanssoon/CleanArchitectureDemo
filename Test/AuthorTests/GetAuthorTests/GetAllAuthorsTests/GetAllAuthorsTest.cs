using Application.Queries.Authors.GetAuthor.GetAll;
using Domain;
using Infrastructure.Database;

namespace Test.AuthorTests.GetAuthorTests.GetAllAuthorsTests
{
    [TestFixture]
    public class GetAllAuthorsTest
    {
        private FakeDatabase fakeDatabase;
        private GetAllAuthorsFromDBQueryHandler handler;

        [SetUp]
        public void Setup()
        {
            fakeDatabase = new FakeDatabase();
            handler = new GetAllAuthorsFromDBQueryHandler(fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldReturnAuthors_WhenAuthorsExist()
        {
            // Arrange
            var newauthor = new Author(4, "Anton Johansson", "Sport");
            fakeDatabase.Authors.Add(newauthor);

            var query = new GetAllAuthorsFromDBQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.AreEqual(4, result.Count);
            Assert.Contains(newauthor, result);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorsListIsEmpty()
        {
            // Arrange
            fakeDatabase.Authors.Clear();
            var query = new GetAllAuthorsFromDBQuery(); // Skapa queryn för att hämta alla författare

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.AreEqual("Your list of authors is empty", exception.Message);
        }
    }
}
