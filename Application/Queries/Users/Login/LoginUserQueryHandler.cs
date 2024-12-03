using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Users.Login.Helpers;
using Domain;
using MediatR;

namespace Application.Queries.Users.Login
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, OperationResult<string>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly TokenHelper _tokenHelper;

        public LoginUserQueryHandler(IRepository<User> userRepository, TokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
        }

        public Task<OperationResult<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var validationResult = ValidateLogin(request.LoginUser);
            if (!validationResult.IsSuccessfull)
            {
                return Task.FromResult(validationResult);
            }

            var user = _userRepository
                .GetAll()
                .FirstOrDefault(u =>
                    u.Username == request.LoginUser.Username &&
                    u.Password == request.LoginUser.Password);

            if (user == null)
            {
                return Task.FromResult(OperationResult<string>.Failure("Invalid username or password!"));
            }

            string token = _tokenHelper.GenerateJwtToken(user);

            return Task.FromResult(OperationResult<string>.Successfull(token, "Login successful!"));
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
