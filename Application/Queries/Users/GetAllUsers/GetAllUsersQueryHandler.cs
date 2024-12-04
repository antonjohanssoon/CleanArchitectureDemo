using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, OperationResult<List<User>>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersQueryHandler(IRepository<User> userRepository, ILogger<GetAllUsersQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling request to fetch all users.");

                var users = _userRepository.GetAll().ToList();

                if (users.Any())
                {
                    _logger.LogInformation("Successfully fetched {UserCount} users.", users.Count);
                    return OperationResult<List<User>>.Successfull(users);
                }

                _logger.LogWarning("No users found.");
                return OperationResult<List<User>>.Failure("Your list of users is empty...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the users.");

                return OperationResult<List<User>>.Failure("An unexpected error occurred while fetching the users.");
            }
        }
    }
}


