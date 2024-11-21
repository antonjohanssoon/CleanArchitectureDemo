namespace Test.UserTests
{
    using Application.Commands.Users.AddUser;
    using Domain;
    using Infrastructure.Database;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    namespace YourNamespace.Tests
    {
        [TestFixture]
        public class AddUserCommandHandlerTests
        {
            private FakeDatabase fakeDatabase;
            private AddUserCommandHandler handler;

            [SetUp]
            public void Setup()
            {
                fakeDatabase = new FakeDatabase();
                handler = new AddUserCommandHandler(fakeDatabase);
            }

            [Test]
            public async Task Handle_ShouldAddUser_WhenUserIsValid()
            {
                // Arrange
                fakeDatabase.Users.Clear();
                var newUser = new User(Guid.NewGuid(), "newUser", "password123");
                var command = new AddUserCommand(newUser);

                // Act
                var result = await handler.Handle(command, CancellationToken.None);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(newUser.Username, result.Username);
                Assert.AreEqual(newUser.Password, result.Password);
                Assert.AreEqual(1, fakeDatabase.Users.Count);
                Assert.Contains(result, fakeDatabase.Users);
            }


            [Test]
            public void Handle_ShouldThrowException_WhenUsernameIsEmpty()
            {
                // Arrange
                var newUser = new User(Guid.NewGuid(), "", "password123");
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
                var newUser = new User(Guid.NewGuid(), "newUser", "");
                var command = new AddUserCommand(newUser);

                // Act
                var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

                // Assert
                Assert.AreEqual("Password is required and cannot be empty.", exception.Message);
            }


            [Test]
            public void Handle_ShouldThrowException_WhenUsernameAlreadyExists()
            {
                // Arrange
                var existingUser = new User(Guid.NewGuid(), "existingUser", "password123");
                fakeDatabase.Users.Add(existingUser);

                var newUser = new User(Guid.NewGuid(), "existingUser", "password456");
                var command = new AddUserCommand(newUser);

                // Act
                var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

                // Assert
                Assert.AreEqual($"User with username '{existingUser.Username}' already exists.", exception.Message);
            }


            [Test]
            public void Handle_ShouldThrowException_WhenUserIdAlreadyExists()
            {
                // Arrange
                var existingUser = new User(Guid.NewGuid(), "existingUser", "password123");
                fakeDatabase.Users.Add(existingUser);

                var newUser = new User(existingUser.Id, "newUser", "password456");
                var command = new AddUserCommand(newUser);

                // Act
                var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

                //Assert
                Assert.AreEqual($"User with ID '{existingUser.Id}' already exists.", exception.Message);
            }
        }
    }

}
