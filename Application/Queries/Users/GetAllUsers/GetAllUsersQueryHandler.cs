using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
    {
        private readonly FakeDatabase fakeDatabase;

        public GetAllUsersQueryHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            if (fakeDatabase.Users.Count == 0)
            {
                throw new Exception($"Your list of users is empty");
            }

            return Task.FromResult(fakeDatabase.Users);
        }
    }
}
