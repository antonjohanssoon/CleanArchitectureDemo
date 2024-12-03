using Application.Commands.Books.DeleteBook;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Moq;

namespace Test.BookTests.DeleteBookTests
{
    [TestFixture]
    public class DeleteBookByIdCommandHandlerTests
    {
        private Mock<IRepository<Book>> mockBookRepository;
        private DeleteBookByIdCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            // Mocka IRepository<Book>
            mockBookRepository = new Mock<IRepository<Book>>();
            handler = new DeleteBookByIdCommandHandler(mockBookRepository.Object);
        }

        [Test]
        public async Task Handle_ShouldDeleteBook_WhenBookExists()
        {
            // Arrange
            var bookToDelete = new Book("Anton Johansson - My Biography", "A book about my life");
            var command = new DeleteBookByIdCommand(bookToDelete.Id);

            // Mocka att GetById returnerar boken som vi ska ta bort
            mockBookRepository.Setup(repo => repo.GetById(bookToDelete.Id)).Returns(bookToDelete);
            mockBookRepository.Setup(repo => repo.Delete(bookToDelete)).Verifiable();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mockBookRepository.Verify(repo => repo.Delete(bookToDelete), Times.Once); // Verifiera att Delete anropades exakt en gång
            Assert.AreEqual(bookToDelete.Id, result.Data.Id);
            Assert.AreEqual(bookToDelete.Title, result.Data.Title);
            Assert.AreEqual(bookToDelete.Description, result.Data.Description);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookNotFound()
        {
            // Arrange
            var nonExistentBookId = Guid.NewGuid();
            var command = new DeleteBookByIdCommand(nonExistentBookId);

            // Mocka att GetById returnerar null för icke-existerande bok
            mockBookRepository.Setup(repo => repo.GetById(nonExistentBookId)).Returns((Book)null);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Book with ID: {nonExistentBookId} not found.", exception.Message);
        }
    }
}
