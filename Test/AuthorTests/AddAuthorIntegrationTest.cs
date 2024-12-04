using Application.Commands.Authors.AddAuthor;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using FakeItEasy;

namespace Test.AuthorTests
{
    [TestFixture]
    public class AddAuthorIntegrationTests
    {
        private IRepository<Author> _fakeRepository;
        private AddAuthorCommandHandler _handler;
        private List<Author> _authorStorage;

        [SetUp]
        public void SetUp()
        {
            _authorStorage = new List<Author>();
            _fakeRepository = A.Fake<IRepository<Author>>();

            A.CallTo(() => _fakeRepository.Add(A<Author>._))
                .Invokes((Author author) => _authorStorage.Add(author));

            A.CallTo(() => _fakeRepository.GetAll())
                .Returns(_authorStorage);

            _handler = new AddAuthorCommandHandler(_fakeRepository);
        }

        [Test]
        public async Task AddAuthor_ValidInput_AddsAuthorToRepository()
        {
            // Arrange
            var newAuthor = new Author("Stephen King", "Horror");
            var command = new AddAuthorCommand(newAuthor);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccessfull, "Operation should be successful.");
            Assert.AreEqual("Author added successfully.", result.Message);
            Assert.AreEqual(1, _authorStorage.Count, "Repository should contain one author.");
            var addedAuthor = _authorStorage.FirstOrDefault(a => a.Name == "Stephen King");
            Assert.IsNotNull(addedAuthor, "Author should be added to the repository.");
            Assert.AreEqual("Horror", addedAuthor.BookCategory, "Book category should match.");
        }

        [Test]
        public async Task AddAuthor_DuplicateName_ReturnsFailure()
        {
            // Arrange
            var existingAuthor = new Author("Stephen King", "Horror");
            _authorStorage.Add(existingAuthor);

            var newAuthor = new Author("Stephen King", "Drama");
            var command = new AddAuthorCommand(newAuthor);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccessfull, "Operation should fail due to duplicate author.");
            Assert.AreEqual($"Author with name '{newAuthor.Name}' already exists.", result.ErrorMessage);
            Assert.AreEqual(1, _authorStorage.Count, "Repository should still contain only the original author.");
        }

        [Test]
        public async Task AddAuthor_InvalidInput_ReturnsFailure()
        {
            // Arrange
            var invalidAuthor = new Author("", "Horror");
            var command = new AddAuthorCommand(invalidAuthor);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccessfull, "Operation should fail due to invalid input.");
            Assert.AreEqual("Author name is required and cannot be empty.", result.ErrorMessage);
            Assert.AreEqual(0, _authorStorage.Count, "Repository should not contain any authors.");
        }
    }

}
