using Application.Dtos;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorByIdCommandHandler : IRequestHandler<UpdateAuthorByIdCommand, OperationResult<Author>>
    {
        private readonly IRepository<Author> _authorRepository;

        public UpdateAuthorByIdCommandHandler(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<OperationResult<Author>> Handle(UpdateAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            var authorToUpdate = _authorRepository.GetById(request.Id);
            if (authorToUpdate == null)
            {
                return Task.FromResult(OperationResult<Author>.Failure($"Author with ID {request.Id} not found."));
            }

            // Validera uppdaterade författarens information
            var validationResult = ValidateUpdatedAuthor(request.UpdatedAuthor);
            if (!validationResult.IsSuccessfull)
            {
                return Task.FromResult(validationResult);
            }

            UpdateAuthorDetails(authorToUpdate, request.UpdatedAuthor);

            _authorRepository.Update(authorToUpdate);

            return Task.FromResult(OperationResult<Author>.Successfull(authorToUpdate, "Author updated successfully."));
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
