using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Queries.Authors.GetAuthor.GetAll
{
    public class GetAllAuthorsFromDBQueryHandler : IRequestHandler<GetAllAuthorsFromDBQuery, List<Author>>
    {
        private readonly IRepository<Author> _authorRepository;

        public GetAllAuthorsFromDBQueryHandler(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<List<Author>> Handle(GetAllAuthorsFromDBQuery request, CancellationToken cancellationToken)
        {
            var authors = _authorRepository.GetAll().ToList();

            if (authors.Count == 0)
            {
                throw new Exception("Your list of authors is empty");
            }

            return Task.FromResult(authors);
        }
    }

}
