using Application.Commands.Authors.AddAuthor;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Moq;

namespace Test.AuthorTests.AddAuthorTests
{

    [TestFixture]
    public class AddAuthorCommandHandlerTests
    {
        private Mock<IRepository<Author>> mockAuthorRepository;
        private AddAuthorCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            // Mocka IRepository<Author>
            mockAuthorRepository = new Mock<IRepository<Author>>();
            handler = new AddAuthorCommandHandler(mockAuthorRepository.Object);
        }

        [Test]
        public async Task Handle_ShouldAddAuthor_WhenAuthorIsValid()
        {
            // Arrange
            var newAuthor = new Author
            {
                Name = "Author Name",
                BookCategory = "Fiction"
            };
            var command = new AddAuthorCommand(newAuthor);

            // Mocka att Add metoden anropas (vi ignorerar implementationen för nu)
            mockAuthorRepository.Setup(repo => repo.Add(It.IsAny<Author>())).Verifiable();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mockAuthorRepository.Verify(repo => repo.Add(It.IsAny<Author>()), Times.Once); // Verifiera att Add anropades exakt en gång
            Assert.AreEqual(newAuthor.Name, result.Data.Name);
            Assert.AreEqual(newAuthor.BookCategory, result.Data.BookCategory);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorNameIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidAuthor = new Author
            {
                Name = "",
                BookCategory = "Fiction"
            };
            var command = new AddAuthorCommand(invalidAuthor);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual("Author name is required and cannot be empty.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorBookCategoryIsEmptyOrWhitespace()
        {
            // Arrange
            var invalidAuthor = new Author
            {
                Name = "Author Name",
                BookCategory = ""
            };
            var command = new AddAuthorCommand(invalidAuthor);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual("Author book category is required and cannot be empty.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenAuthorWithSameNameExists()
        {
            // Arrange
            var existingAuthor = new Author("Existing Author", "Fiction");
            var newAuthor = new Author
            {
                Name = "Existing Author",
                BookCategory = "Non-Fiction"
            };
            var command = new AddAuthorCommand(newAuthor);

            mockAuthorRepository.Setup(repo => repo.GetAll()).Returns(new[] { existingAuthor });

            // Act 
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"Author with name '{newAuthor.Name}' already exists.", exception.Message);
        }
    }


}
