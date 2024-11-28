using Application.Dtos;
using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Users.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, User>
    {
        private readonly FakeDatabase fakeDatabase;

        public AddUserCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<User> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            ValidateUser(request.NewUser);
            CheckForDuplicateUser(request.NewUser);

            var newUser = new User(request.NewUser.Username, request.NewUser.Password);
            fakeDatabase.Users.Add(newUser);

            return Task.FromResult(newUser);
        }

        private void ValidateUser(UserDto user)
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

        private void CheckForDuplicateUser(UserDto user)
        {
            if (fakeDatabase.Users.Any(existingUser => existingUser.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"User with username '{user.Username}' already exists.");
            }
        }
    }
}

