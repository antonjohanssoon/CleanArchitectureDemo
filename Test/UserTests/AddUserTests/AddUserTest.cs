namespace Test.UserTests
{
    using Application.Commands.Users.AddUser;
    using Application.Dtos;
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
                var newUserDto = new UserDto
                {
                    Username = "newUser",
                    Password = "password123"
                };
                var command = new AddUserCommand(newUserDto);

                // Act
                var result = await handler.Handle(command, CancellationToken.None);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(newUserDto.Username, result.Username);
                Assert.AreEqual(newUserDto.Password, result.Password);
                Assert.AreEqual(1, fakeDatabase.Users.Count);
                Assert.Contains(result, fakeDatabase.Users);
            }

            [Test]
            public void Handle_ShouldThrowException_WhenUsernameIsEmpty()
            {
                // Arrange
                var newUserDto = new UserDto
                {
                    Username = "",
                    Password = "password123"
                };
                var command = new AddUserCommand(newUserDto);

                // Act
                var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

                // Assert
                Assert.AreEqual("Username is required and cannot be empty.", exception.Message);
            }

            [Test]
            public void Handle_ShouldThrowException_WhenPasswordIsEmpty()
            {
                // Arrange
                var newUserDto = new UserDto
                {
                    Username = "newUser",
                    Password = ""
                };
                var command = new AddUserCommand(newUserDto);

                // Act
                var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

                // Assert
                Assert.AreEqual("Password is required and cannot be empty.", exception.Message);
            }

            [Test]
            public void Handle_ShouldThrowException_WhenUsernameAlreadyExists()
            {
                // Arrange
                var existingUser = new User("existingUser", "password123");
                fakeDatabase.Users.Add(existingUser);

                var newUserDto = new UserDto
                {
                    Username = "existingUser",
                    Password = "password456"
                };
                var command = new AddUserCommand(newUserDto);

                // Act
                var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

                // Assert
                Assert.AreEqual($"User with username '{existingUser.Username}' already exists.", exception.Message);
            }
        }

    }

}
