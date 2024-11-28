using Application.Commands.Authors.DeleteAuthor;
using Domain;
using Infrastructure.Database;

namespace Test.AuthorTests.DeleteAuthorTests
{
    [TestFixture]
    public class DeleteAuthorByIdCommandHandlerTests
    {
        private FakeDatabase fakeDatabase;
        private DeleteAuthorByIdCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            fakeDatabase = new FakeDatabase();
            handler = new DeleteAuthorByIdCommandHandler(fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldDeleteAuthor_WhenAuthorExists()
        {
            // Arrange
            var authorToDelete = new Author("Anton Johansson", "Sport");
            fakeDatabase.Authors.Add(authorToDelete);
            var command = new DeleteAuthorByIdCommand(authorToDelete.Id);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(authorToDelete.Name, result.Name);
            Assert.IsFalse(fakeDatabase.Authors.Contains(authorToDelete));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorNotFound()
        {
            // Arrange
            var nonExistentAuthorId = Guid.NewGuid();
            var command = new DeleteAuthorByIdCommand(nonExistentAuthorId);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Author with ID: {nonExistentAuthorId} not found.", exception.Message);
        }
    }

}
