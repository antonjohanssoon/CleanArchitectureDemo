using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Users.Login;
using Application.Queries.Users.Login.Helpers;
using Domain;
using FakeItEasy;


namespace Test.UserTests
{
    [TestFixture]
    public class LoginUserTests
    {
        private IRepository<User> _userRepository;
        private TokenHelper _tokenHelper;

        [SetUp]
        public void SetUp()
        {
            _userRepository = A.Fake<IRepository<User>>();
            _tokenHelper = A.Fake<TokenHelper>();
        }

        [Test]
        public async Task Handle_ValidLogin_ReturnsSuccessfulResultWithToken()
        {
            // Arrange
            var users = new List<User>
        {
            new User("anton123", "anton123") { Id = Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6") }
        };
            A.CallTo(() => _userRepository.GetAll()).Returns(users);

            var expectedToken = "fake-jwt-token";
            A.CallTo(() => _tokenHelper.GenerateJwtToken(A<User>.Ignored)).Returns(expectedToken);

            var query = new LoginUserQuery(new User { Username = "anton123", Password = "anton123" });
            var handler = new LoginUserQueryHandler(_userRepository, _tokenHelper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccessfull);
            Assert.AreEqual("Login successful!", result.Message);
            Assert.AreEqual(expectedToken, result.Data);
        }

        [Test]
        public async Task Handle_InvalidUsernameOrPassword_ReturnsFailure()
        {
            // Arrange
            var users = new List<User>
        {
            new User("anton123", "anton123") { Id = Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6") }
        };
            A.CallTo(() => _userRepository.GetAll()).Returns(users);

            var query = new LoginUserQuery(new User { Username = "invalidUser", Password = "wrongPassword" });
            var handler = new LoginUserQueryHandler(_userRepository, _tokenHelper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccessfull);
            Assert.AreEqual("Invalid username or password!", result.ErrorMessage);
        }

        [Test]
        public async Task Handle_EmptyUsername_ReturnsFailure()
        {
            // Arrange
            var query = new LoginUserQuery(new User { Username = "", Password = "anton123" });
            var handler = new LoginUserQueryHandler(_userRepository, _tokenHelper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccessfull);
            Assert.AreEqual("Username is required and cannot be empty.", result.ErrorMessage);
        }

        [Test]
        public async Task Handle_EmptyPassword_ReturnsFailure()
        {
            // Arrange
            var query = new LoginUserQuery(new User { Username = "anton123", Password = "" });
            var handler = new LoginUserQueryHandler(_userRepository, _tokenHelper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccessfull);
            Assert.AreEqual("Password is required and cannot be empty.", result.ErrorMessage);
        }
    }

}
