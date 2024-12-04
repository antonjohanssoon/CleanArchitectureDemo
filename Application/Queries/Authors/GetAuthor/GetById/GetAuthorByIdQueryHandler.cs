using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Authors.GetAuthor.GetById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, OperationResult<Author>>
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly ILogger<GetAuthorByIdQueryHandler> _logger;

        public GetAuthorByIdQueryHandler(IRepository<Author> authorRepository, ILogger<GetAuthorByIdQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Author>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling request to fetch author with ID: {AuthorId}", request.Id);

                var author = _authorRepository.GetById(request.Id);

                if (author != null)
                {
                    _logger.LogInformation("Author with ID: {AuthorId} found.", request.Id);
                    return OperationResult<Author>.Successfull(author);
                }

                _logger.LogWarning("Author with ID: {AuthorId} not found.", request.Id);
                return OperationResult<Author>.Failure($"Author with ID: {request.Id} was not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the author with ID: {AuthorId}", request.Id);
                return OperationResult<Author>.Failure("An unexpected error occurred while fetching the author.");
            }
        }
    }
}

