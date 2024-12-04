using Application.Commands.Books.DeleteBook;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace Test.BookTests
{
    [TestFixture]
    public class DeleteBookByIdIntegrationTests
    {
        private IRepository<Book> _fakeRepository;
        private DeleteBookByIdCommandHandler _handler;
        private List<Book> _bookStorage;
        private ILogger<DeleteBookByIdCommandHandler> _fakeLogger;

        [SetUp]
        public void SetUp()
        {
            _bookStorage = new List<Book>();
            _fakeRepository = A.Fake<IRepository<Book>>();
            _fakeLogger = A.Fake<ILogger<DeleteBookByIdCommandHandler>>();

            A.CallTo(() => _fakeRepository.GetById(A<Guid>._))
                .ReturnsLazily((Guid id) => _bookStorage.FirstOrDefault(b => b.Id == id));

            A.CallTo(() => _fakeRepository.Delete(A<Book>._))
                .Invokes((Book book) => _bookStorage.Remove(book));

            _handler = new DeleteBookByIdCommandHandler(_fakeRepository, _fakeLogger);
        }

        [Test]
        public async Task DeleteBook_BookExists_DeletesBookAndReturnsSuccess()
        {
            // Arrange
            var bookToDelete = new Book("Book Title", "Book Description")
            {
                Id = Guid.NewGuid()
            };
            _bookStorage.Add(bookToDelete);

            var command = new DeleteBookByIdCommand(bookToDelete.Id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccessfull, "Operation should be successful.");
            Assert.AreEqual(bookToDelete, result.Data, "Deleted book should match the one requested.");
            Assert.AreEqual(0, _bookStorage.Count, "Book storage should be empty after deletion.");
        }

        [Test]
        public async Task DeleteBook_BookDoesNotExist_ReturnsFailure()
        {
            // Arrange
            var nonExistentBookId = Guid.NewGuid();
            var command = new DeleteBookByIdCommand(nonExistentBookId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccessfull, "Operation should fail when book does not exist.");
            Assert.AreEqual($"Book with ID: {nonExistentBookId} was not found.", result.ErrorMessage);
            Assert.AreEqual(0, _bookStorage.Count, "Book storage should remain empty.");
        }
    }

}
