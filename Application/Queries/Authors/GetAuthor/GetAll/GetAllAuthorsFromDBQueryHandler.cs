using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Authors.GetAuthor.GetAll
{
    public class GetAllAuthorsFromDBQueryHandler : IRequestHandler<GetAllAuthorsFromDBQuery, OperationResult<List<Author>>>
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly ILogger<GetAllAuthorsFromDBQueryHandler> _logger;

        public GetAllAuthorsFromDBQueryHandler(IRepository<Author> authorRepository, ILogger<GetAllAuthorsFromDBQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<Author>>> Handle(GetAllAuthorsFromDBQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling request to fetch all authors.");

                var authors = _authorRepository.GetAll().ToList();

                if (authors.Any())
                {
                    _logger.LogInformation("{AuthorCount} authors found.", authors.Count);
                    return OperationResult<List<Author>>.Successfull(authors);
                }

                _logger.LogWarning("No authors found in the database. Returning an empty list.");
                return OperationResult<List<Author>>.Failure("Your list of authors is empty...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching authors from the database.");
                return OperationResult<List<Author>>.Failure("An unexpected error occurred while fetching authors.");
            }
        }
    }
}

