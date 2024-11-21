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
        public async Task Handle_ShouldDeleteBook_WhenAuthorExists()
        {
            // Arrange
            var bookToDelete = new Book(9, "Anton Johansson - my biography", "A book about me");
            fakeDatabase.Books.Add(bookToDelete);
            var command = new DeleteBookByIdCommand(9);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(bookToDelete.Title, result.Title);
            Assert.IsFalse(fakeDatabase.Books.Contains(bookToDelete));
        }

        [Test]
        public void Handle_ShouldThrowException_WhenBookNotFound()
        {
            // Arrange
            var command = new DeleteBookByIdCommand(999);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            //Assert
            Assert.AreEqual("Book with ID: 999 not found.", exception.Message);
        }
    }
}
