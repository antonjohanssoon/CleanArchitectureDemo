using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Users.Login.Helpers;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Users.Login
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, OperationResult<string>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<LoginUserQueryHandler> _logger;

        public LoginUserQueryHandler(IRepository<User> userRepository, TokenHelper tokenHelper, ILogger<LoginUserQueryHandler> logger)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling login request for user: {Username}", request.LoginUser.Username);

                var validationResult = ValidateLogin(request.LoginUser);
                if (!validationResult.IsSuccessfull)
                {
                    _logger.LogWarning("Login failed for user: {Username}. Validation error: {ErrorMessage}", request.LoginUser.Username, validationResult.ErrorMessage);
                    return validationResult;
                }

                var user = _userRepository
                    .GetAll()
                    .FirstOrDefault(u =>
                        u.Username == request.LoginUser.Username &&
                        u.Password == request.LoginUser.Password);

                if (user == null)
                {
                    _logger.LogWarning("Login failed for user: {Username}. Invalid credentials.", request.LoginUser.Username);
                    return OperationResult<string>.Failure("Invalid username or password!");
                }

                string token = _tokenHelper.GenerateJwtToken(user);
                _logger.LogInformation("Login successful for user: {Username}. JWT token generated.", request.LoginUser.Username);

                return OperationResult<string>.Successfull(token, "Login successful!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the login request for user: {Username}", request.LoginUser.Username);

                return OperationResult<string>.Failure("An unexpected error occurred during the login process.");
            }
        }

        private OperationResult<string> ValidateLogin(User loginUser)
        {
            if (string.IsNullOrWhiteSpace(loginUser.Username))
            {
                return OperationResult<string>.Failure("Username is required and cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(loginUser.Password))
            {
                return OperationResult<string>.Failure("Password is required and cannot be empty.");
            }

            return OperationResult<string>.Successfull(null);
        }
    }
}

