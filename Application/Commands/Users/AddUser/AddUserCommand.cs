using Domain;
using MediatR;

namespace Application.Commands.Users.AddUser
{
    public class AddUserCommand : IRequest<User>
    {
        public AddUserCommand(User newUser)
        {
            NewUser = newUser;
        }
        public User NewUser { get; set; }
    }
}
