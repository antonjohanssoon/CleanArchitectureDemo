using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Queries.Authors.GetAuthor.GetAll
{
    public class GetAllAuthorsFromDBQueryHandler : IRequestHandler<GetAllAuthorsFromDBQuery, OperationResult<List<Author>>>
    {
        private readonly IRepository<Author> _authorRepository;

        public GetAllAuthorsFromDBQueryHandler(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<OperationResult<List<Author>>> Handle(GetAllAuthorsFromDBQuery request, CancellationToken cancellationToken)
        {
            var authors = _authorRepository.GetAll().ToList();

            if (authors.Any())
            {
                return Task.FromResult(OperationResult<List<Author>>.Successfull(authors));
            }

            return Task.FromResult(OperationResult<List<Author>>.Failure($"Your list of authors are empty..."));
        }
    }

}
