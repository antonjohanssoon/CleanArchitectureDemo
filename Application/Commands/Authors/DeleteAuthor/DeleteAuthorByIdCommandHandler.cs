using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Authors.DeleteAuthor
{
    public class DeleteAuthorByIdCommandHandler : IRequestHandler<DeleteAuthorByIdCommand, OperationResult<Author>>
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly ILogger<DeleteAuthorByIdCommandHandler> _logger;

        public DeleteAuthorByIdCommandHandler(IRepository<Author> authorRepository, ILogger<DeleteAuthorByIdCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Author>> Handle(DeleteAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling DeleteAuthorByIdCommand for author with ID: {AuthorId}", request.Id);

                var authorToDelete = _authorRepository.GetById(request.Id);

                if (authorToDelete != null)
                {
                    _authorRepository.Delete(authorToDelete);

                    _logger.LogInformation("Author with ID: {AuthorId} was successfully deleted.", request.Id);

                    return OperationResult<Author>.Successfull(authorToDelete, "Author deleted successfully.");
                }

                _logger.LogWarning("Attempted to delete author with ID: {AuthorId}, but no author was found.", request.Id);

                return OperationResult<Author>.Failure($"Author with ID: {request.Id} was not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to delete author with ID: {AuthorId}", request.Id);
                return OperationResult<Author>.Failure("An unexpected error occurred while deleting the author.");
            }
        }
    }
}

