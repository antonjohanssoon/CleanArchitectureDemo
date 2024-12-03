using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Commands.Users.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, OperationResult<User>>
    {
        private readonly IRepository<User> _userRepository;

        public AddUserCommandHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<OperationResult<User>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = ValidateUser(request.NewUser);
            if (!validationResult.IsSuccessfull)
            {
                return Task.FromResult(validationResult);
            }

            var duplicateCheckResult = CheckForDuplicateUser(request.NewUser);
            if (!duplicateCheckResult.IsSuccessfull)
            {
                return Task.FromResult(duplicateCheckResult);
            }

            _userRepository.Add(request.NewUser);

            return Task.FromResult(OperationResult<User>.Successfull(request.NewUser, "User added successfully."));
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

