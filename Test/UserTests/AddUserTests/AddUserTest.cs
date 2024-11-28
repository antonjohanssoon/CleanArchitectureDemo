using Application.Commands.Users.AddUser;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Moq;

namespace Test.UserTests
{
    [TestFixture]
    public class AddUserCommandHandlerTests
    {
        private Mock<IRepository<User>> mockUserRepository;
        private AddUserCommandHandler handler;

        [SetUp]
        public void Setup()
        {
            mockUserRepository = new Mock<IRepository<User>>();
            handler = new AddUserCommandHandler(mockUserRepository.Object);
        }

        [Test]
        public async Task Handle_ShouldAddUser_WhenUserIsValid()
        {
            // Arrange
            var newUser = new User
            {
                Username = "newUser",
                Password = "password123"
            };
            var command = new AddUserCommand(newUser);

            mockUserRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns((User)null);

            mockUserRepository.Setup(repo => repo.Add(It.IsAny<User>())).Verifiable();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mockUserRepository.Verify(repo => repo.Add(It.IsAny<User>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(newUser.Username, result.Username);
            Assert.AreEqual(newUser.Password, result.Password);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenUsernameIsEmpty()
        {
            // Arrange
            var newUser = new User
            {
                Username = "",
                Password = "password123"
            };
            var command = new AddUserCommand(newUser);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual("Username is required and cannot be empty.", exception.Message);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenPasswordIsEmpty()
        {
            // Arrange
            var newUser = new User
            {
                Username = "newUser",
                Password = ""
            };
            var command = new AddUserCommand(newUser);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual("Password is required and cannot be empty.", exception.Message);
        }

        [Test]
        public async Task Handle_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            // Arrange
            var existingUser = new User("existingUser", "password123");

            // Mocka GetAll utan att inkludera CancellationToken
            mockUserRepository.Setup(repo => repo.GetAll()).Returns(new List<User> { existingUser });

            var newUser = new User
            {
                Username = "existingUser",
                Password = "password456"
            };
            var command = new AddUserCommand(newUser);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual($"User with username '{existingUser.Username}' already exists.", exception.Message);
        }
    }
}

