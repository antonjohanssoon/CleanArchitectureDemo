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
            return Task.FromResult(fakeDatabase.Authors);
        }
    }
}
