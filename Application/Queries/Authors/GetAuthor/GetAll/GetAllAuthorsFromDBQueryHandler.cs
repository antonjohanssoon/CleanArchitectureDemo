using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Queries.Authors.GetAuthor.GetAll
{
    public class GetAllAuthorsFromDBQueryHandler : IRequestHandler<GetAllAuthorsFromDBQuery, List<Author>>
    {
        private readonly FakeDatabase fakeDatabase;

        public GetAllAuthorsFromDBQueryHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<List<Author>> Handle(GetAllAuthorsFromDBQuery request, CancellationToken cancellationToken)
        {
            if (fakeDatabase.Authors.Count == 0)
            {
                throw new Exception($"Your list of authors is empty");
            }

            return Task.FromResult(fakeDatabase.Authors);
        }
    }
}
