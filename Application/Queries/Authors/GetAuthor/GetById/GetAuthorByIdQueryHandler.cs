using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Queries.Authors.GetAuthor.GetById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Author>
    {
        private readonly FakeDatabase fakeDatabase;

        public GetAuthorByIdQueryHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var author = fakeDatabase.Authors.FirstOrDefault(author => author.Id == request.Id);

            if (author == null)
            {
                throw new KeyNotFoundException($"Author with ID: {request.Id} was not found.");
            }

            return Task.FromResult(author);
        }
    }
}
