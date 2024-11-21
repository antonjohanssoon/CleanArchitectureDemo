using Application.Queries.Users.Login.Helpers;
using Infrastructure.Database;
using MediatR;

namespace Application.Queries.Users.Login
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
    {
        private readonly FakeDatabase fakeDatabase;
        private readonly TokenHelper tokenHelper;

        public LoginUserQueryHandler(FakeDatabase fakeDatabase, TokenHelper tokenHelper)
        {
            this.fakeDatabase = fakeDatabase;
            this.tokenHelper = tokenHelper;
        }

        public Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = fakeDatabase.Users.FirstOrDefault(user => user.Username == request.LoginUser.Username && user.Password == request.LoginUser.Password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password!");
            }

            string token = tokenHelper.GenerateJwtToken(user);

            return Task.FromResult(token);
        }
    }
}
