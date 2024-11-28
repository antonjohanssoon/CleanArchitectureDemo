using Application.Dtos;
using Domain;
using MediatR;

namespace Application.Commands.Users.AddUser
{
    public class AddUserCommand : IRequest<User>
    {
        private UserDto userDto;
        public AddUserCommand(User newUser)
        {
            NewUser = newUser;
        }

        public AddUserCommand(UserDto userDto)
        {
            this.userDto = userDto;
        }

        public User NewUser { get; set; }
    }
}
