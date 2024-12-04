using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Authors.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, OperationResult<Author>>
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly ILogger<AddAuthorCommandHandler> _logger;

        public AddAuthorCommandHandler(IRepository<Author> authorRepository, ILogger<AddAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Author>> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling AddAuthorCommand for author: {AuthorName}", request.NewAuthor.Name);

                var validationResult = ValidateAuthor(request.NewAuthor);
                if (!validationResult.IsSuccessfull)
                {
                    _logger.LogWarning("Validation failed for author: {AuthorName}. Error: {ErrorMessage}",
                        request.NewAuthor.Name, validationResult.ErrorMessage);
                    return validationResult;
                }

                var duplicateCheckResult = CheckForDuplicateAuthor(request.NewAuthor);
                if (!duplicateCheckResult.IsSuccessfull)
                {
                    _logger.LogWarning("Duplicate check failed for author: {AuthorName}. Error: {ErrorMessage}",
                        request.NewAuthor.Name, duplicateCheckResult.ErrorMessage);
                    return duplicateCheckResult;
                }

                await _authorRepository.Add(request.NewAuthor);
                _logger.LogInformation("Author {AuthorName} added successfully.", request.NewAuthor.Name);

                return OperationResult<Author>.Successfull(request.NewAuthor, "Author added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the author: {AuthorName}", request.NewAuthor.Name);
                return OperationResult<Author>.Failure("An unexpected error occurred while adding the author.");
            }
        }

        private OperationResult<Author> ValidateAuthor(Author author)
        {
            if (string.IsNullOrWhiteSpace(author.Name))
            {
                return OperationResult<Author>.Failure("Name is required and cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(author.BookCategory))
            {
                return OperationResult<Author>.Failure("Book category is required and cannot be empty.");
            }

            return OperationResult<Author>.Successfull(author);
        }

        private OperationResult<Author> CheckForDuplicateAuthor(Author author)
        {
            var existingAuthors = _authorRepository.GetAll();
            if (existingAuthors.Any(existingAuthor =>
                existingAuthor.Name.Equals(author.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return OperationResult<Author>.Failure($"Author with name '{author.Name}' already exists.");
            }

            return OperationResult<Author>.Successfull(author);
        }
    }
}

