using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Users.Login.Helpers;
using Domain;
using MediatR;

namespace Application.Queries.Users.Login
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
    {
        private readonly IRepository<User> _userRepository;
        private readonly TokenHelper _tokenHelper;

        public LoginUserQueryHandler(IRepository<User> userRepository, TokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
        }

        public Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = _userRepository
                .GetAll()
                .FirstOrDefault(u => u.Username == request.LoginUser.Username && u.Password == request.LoginUser.Password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password!");
            }

            string token = _tokenHelper.GenerateJwtToken(user);

            return Task.FromResult(token);
        }
    }

}
