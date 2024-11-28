using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Commands.Users.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, User>
    {
        private readonly IRepository<User> _userRepository;

        public AddUserCommandHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            ValidateUser(request.NewUser);
            CheckForDuplicateUser(request.NewUser);

            _userRepository.Add(request.NewUser);

            return Task.FromResult(request.NewUser);
        }

        private void ValidateUser(User user)
        {

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new Exception("Username is required and cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new Exception("Password is required and cannot be empty.");
            }
        }

        private void CheckForDuplicateUser(User user)
        {
            var existingUsers = _userRepository.GetAll();
            if (existingUsers.Any(existingUser =>
                existingUser.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"User with username '{user.Username}' already exists.");
            }
        }
    }
}

