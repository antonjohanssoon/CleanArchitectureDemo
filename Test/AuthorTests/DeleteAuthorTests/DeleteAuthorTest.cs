using Application.Commands.Authors.DeleteAuthor;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Moq;

namespace Test.AuthorTests.DeleteAuthorTests
{
    [TestFixture]
    public class DeleteAuthorByIdCommandHandlerTests
    {
        private Mock<IRepository<Author>> mockAuthorRepository;
        private DeleteAuthorByIdCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            mockAuthorRepository = new Mock<IRepository<Author>>();
            handler = new DeleteAuthorByIdCommandHandler(mockAuthorRepository.Object);
        }

        [Test]
        public async Task Handle_ShouldDeleteAuthor_WhenAuthorExists()
        {
            // Arrange
            var authorToDelete = new Author("Anton Johansson", "Sport");
            var command = new DeleteAuthorByIdCommand(authorToDelete.Id);

            mockAuthorRepository.Setup(repo => repo.GetById(authorToDelete.Id)).Returns(authorToDelete);
            mockAuthorRepository.Setup(repo => repo.Delete(authorToDelete)).Verifiable();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mockAuthorRepository.Verify(repo => repo.Delete(authorToDelete), Times.Once);
            Assert.AreEqual(authorToDelete.Name, result.Name);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorNotFound()
        {
            // Arrange
            var nonExistentAuthorId = Guid.NewGuid();
            var command = new DeleteAuthorByIdCommand(nonExistentAuthorId);

            mockAuthorRepository.Setup(repo => repo.GetById(nonExistentAuthorId)).Returns((Author)null);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Author with ID: {nonExistentAuthorId} not found.", exception.Message);
        }
    }
}
