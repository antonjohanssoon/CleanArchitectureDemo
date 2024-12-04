using Application.Dtos;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorByIdCommandHandler : IRequestHandler<UpdateAuthorByIdCommand, OperationResult<Author>>
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly ILogger<UpdateAuthorByIdCommandHandler> _logger;

        public UpdateAuthorByIdCommandHandler(IRepository<Author> authorRepository, ILogger<UpdateAuthorByIdCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Author>> Handle(UpdateAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling UpdateAuthorByIdCommand for author with ID: {AuthorId}", request.Id);

                var authorToUpdate = _authorRepository.GetById(request.Id);
                if (authorToUpdate == null)
                {
                    _logger.LogWarning("Attempted to update author with ID: {AuthorId}, but no author was found.", request.Id);
                    return OperationResult<Author>.Failure($"Author with ID {request.Id} not found.");
                }

                var validationResult = ValidateUpdatedAuthor(request.UpdatedAuthor);
                if (!validationResult.IsSuccessfull)
                {
                    _logger.LogWarning("Validation failed for author update. Author name: {AuthorName}, BookCategory: {BookCategory}. Error: {ErrorMessage}",
                        request.UpdatedAuthor.Name, request.UpdatedAuthor.BookCategory, validationResult.ErrorMessage);
                    return validationResult;
                }

                UpdateAuthorDetails(authorToUpdate, request.UpdatedAuthor);
                _authorRepository.Update(authorToUpdate);

                _logger.LogInformation("Author with ID: {AuthorId} was successfully updated.", request.Id);

                return OperationResult<Author>.Successfull(authorToUpdate, "Author updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating author with ID: {AuthorId}", request.Id);
                return OperationResult<Author>.Failure("An unexpected error occurred while updating the author.");
            }
        }

        private OperationResult<Author> ValidateUpdatedAuthor(AuthorDto updatedAuthor)
        {
            if (string.IsNullOrWhiteSpace(updatedAuthor.Name))
            {
                return OperationResult<Author>.Failure("Author name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(updatedAuthor.BookCategory))
            {
                return OperationResult<Author>.Failure("Book category cannot be empty.");
            }

            return OperationResult<Author>.Successfull(null);
        }

        private void UpdateAuthorDetails(Author existingAuthor, AuthorDto updatedAuthor)
        {
            existingAuthor.Name = updatedAuthor.Name;
            existingAuthor.BookCategory = updatedAuthor.BookCategory;
        }
    }
}

