using Domain;
using MediatR;


namespace Application.Queries.Users.Login
{
    public class LoginUserQuery : IRequest<OperationResult<string>>
    {
        public LoginUserQuery(User loginUser)
        {
            LoginUser = loginUser;
        }
        public User LoginUser { get; }
    }
}
