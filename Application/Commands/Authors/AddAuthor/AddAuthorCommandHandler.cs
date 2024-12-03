using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Commands.Authors.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, OperationResult<Author>>
    {
        private readonly IRepository<Author> _authorRepository;

        public AddAuthorCommandHandler(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<OperationResult<Author>> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {

            var validationResult = ValidateAuthor(request.NewAuthor);
            if (!validationResult.IsSuccessfull)
            {
                return Task.FromResult(validationResult);
            }

            var duplicateCheckResult = CheckForDuplicateAuthor(request.NewAuthor);
            if (!duplicateCheckResult.IsSuccessfull)
            {
                return Task.FromResult(duplicateCheckResult);
            }

            _authorRepository.Add(request.NewAuthor);

            return Task.FromResult(OperationResult<Author>.Successfull(request.NewAuthor, "Author added successfully."));
        }

        private OperationResult<Author> ValidateAuthor(Author author)
        {
            if (string.IsNullOrWhiteSpace(author.Name))
            {
                return OperationResult<Author>.Failure("Author name is required and cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(author.BookCategory))
            {
                return OperationResult<Author>.Failure("Author book category is required and cannot be empty.");
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
