using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Users.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, OperationResult<User>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<AddUserCommandHandler> _logger;

        public AddUserCommandHandler(IRepository<User> userRepository, ILogger<AddUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<User>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Attempting to add new user with username: {Username}", request.NewUser.Username);

                var validationResult = ValidateUser(request.NewUser);
                if (!validationResult.IsSuccessfull)
                {
                    _logger.LogWarning("Validation failed for user with username: {Username}. Error: {ErrorMessage}",
                        request.NewUser.Username, validationResult.ErrorMessage);
                    return validationResult;
                }

                var duplicateCheckResult = CheckForDuplicateUser(request.NewUser);
                if (!duplicateCheckResult.IsSuccessfull)
                {
                    _logger.LogWarning("Duplicate check failed for user with username: {Username}. Error: {ErrorMessage}",
                        request.NewUser.Username, duplicateCheckResult.ErrorMessage);
                    return duplicateCheckResult;
                }

                _userRepository.Add(request.NewUser);
                _logger.LogInformation("User with username: {Username} added successfully.", request.NewUser.Username);

                return OperationResult<User>.Successfull(request.NewUser, "User added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to add user with username: {Username}", request.NewUser.Username);
                return OperationResult<User>.Failure("An unexpected error occurred while adding the user.");
            }
        }

        private OperationResult<User> ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                return OperationResult<User>.Failure("Username is required and cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                return OperationResult<User>.Failure("Password is required and cannot be empty.");
            }

            return OperationResult<User>.Successfull(null);
        }

        private OperationResult<User> CheckForDuplicateUser(User user)
        {
            var existingUsers = _userRepository.GetAll();
            if (existingUsers.Any(existingUser =>
                existingUser.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
            {
                return OperationResult<User>.Failure($"User with username '{user.Username}' already exists.");
            }

            return OperationResult<User>.Successfull(null);
        }
    }
}


