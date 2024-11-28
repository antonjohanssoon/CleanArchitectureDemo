using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Queries.Authors.GetAuthor.GetById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Author>
    {
        private readonly IRepository<Author> _authorRepository;

        public GetAuthorByIdQueryHandler(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var author = _authorRepository.GetById(request.Id);

            if (author == null)
            {
                throw new KeyNotFoundException($"Author with ID: {request.Id} was not found.");
            }

            return Task.FromResult(author);
        }
    }
}
