using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Queries.Authors.GetAuthor.GetById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, OperationResult<Author>>
    {
        private readonly IRepository<Author> _authorRepository;

        public GetAuthorByIdQueryHandler(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<OperationResult<Author>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var author = _authorRepository.GetById(request.Id);

            if (author != null)
            {
                return Task.FromResult(OperationResult<Author>.Successfull(author));
            }
            return Task.FromResult(OperationResult<Author>.Failure($"Author with ID: {request.Id} was not found."));
        }
    }
}
