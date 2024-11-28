using Application.Commands.Books.DeleteBook;
using Domain;
using Infrastructure.Database;

namespace Test.BookTests.DeleteBookTests
{
    [TestFixture]
    public class DeleteBookByIdCommandHandlerTests
    {
        private FakeDatabase fakeDatabase;
        private DeleteBookByIdCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            fakeDatabase = new FakeDatabase();
            handler = new DeleteBookByIdCommandHandler(fakeDatabase);
        }

        [Test]
        public async Task Handle_ShouldDeleteBook_WhenBookExists()
        {
            // Arrange
            var bookToDelete = new Book("Anton Johansson - my biography", "A book about me");
            fakeDatabase.Books.Add(bookToDelete);
            var command = new DeleteBookByIdCommand(bookToDelete.Id);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(bookToDelete.Id, result.Id);
            Assert.AreEqual(bookToDelete.Title, result.Title);
            Assert.AreEqual(bookToDelete.Description, result.Description);
            Assert.IsFalse(fakeDatabase.Books.Contains(bookToDelete));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookNotFound()
        {
            // Arrange
            var nonExistentBookId = Guid.NewGuid();
            var command = new DeleteBookByIdCommand(nonExistentBookId);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Book with ID: {nonExistentBookId} not found.", exception.Message);
        }
    }

}
