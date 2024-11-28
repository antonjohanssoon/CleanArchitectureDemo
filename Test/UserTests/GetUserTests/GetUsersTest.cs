namespace Test.UserTests.GetUserTests
{
    using Application.Interfaces.RepositoryInterfaces;
    using Application.Queries.Users.GetAllUsers;
    using Domain;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class GetAllUsersQueryHandlerTests
    {
        private Mock<IRepository<User>> mockUserRepository;
        private GetAllUsersQueryHandler handler;

        [SetUp]
        public void Setup()
        {
            mockUserRepository = new Mock<IRepository<User>>();
            handler = new GetAllUsersQueryHandler(mockUserRepository.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnUsers_WhenUsersExist()
        {
            // Arrange
            var user1 = new User("user1", "password123");
            var user2 = new User("user2", "password456");
            var users = new List<User> { user1, user2 };

            mockUserRepository.Setup(repo => repo.GetAll()).Returns(users);

            var query = new GetAllUsersQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.Contains(user1, result);
            Assert.Contains(user2, result);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenUsersListIsEmpty()
        {
            // Arrange
            mockUserRepository.Setup(repo => repo.GetAll()).Returns(new List<User>());
            var query = new GetAllUsersQuery();

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.AreEqual("Your list of users is empty", exception.Message);
        }
    }
}

