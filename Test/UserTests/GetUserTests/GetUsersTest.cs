namespace Test.UserTests.GetUserTests
{
    using Application.Queries.Users.GetAllUsers;
    using Domain;
    using Infrastructure.Database;
    using NUnit.Framework;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    namespace YourNamespace.Tests
    {
        [TestFixture]
        public class GetAllUsersQueryHandlerTests
        {
            private FakeDatabase fakeDatabase;
            private GetAllUsersQueryHandler handler;

            [SetUp]
            public void Setup()
            {
                fakeDatabase = new FakeDatabase();
                handler = new GetAllUsersQueryHandler(fakeDatabase);
            }

            [Test]
            public async Task Handle_ShouldReturnUsers_WhenUsersExist()
            {
                // Arrange
                fakeDatabase.Users.Clear();
                var user1 = new User("user1", "password123");
                var user2 = new User("user2", "password456");
                fakeDatabase.Users.Add(user1);
                fakeDatabase.Users.Add(user2);

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
                fakeDatabase.Users.Clear();
                var query = new GetAllUsersQuery();

                // Act
                var exception = Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));

                // Assert
                Assert.AreEqual("Your list of users is empty", exception.Message);
            }
        }
    }

}
