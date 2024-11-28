using Application.Dtos;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorByIdCommandHandler : IRequestHandler<UpdateAuthorByIdCommand, Author>
    {
        private readonly IRepository<Author> _authorRepository;

        public UpdateAuthorByIdCommandHandler(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public Task<Author> Handle(UpdateAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            var authorToUpdate = _authorRepository.GetById(request.Id);
            if (authorToUpdate == null)
            {
                throw new Exception($"Author with ID {request.Id} not found.");
            }
            ValidateUpdatedAuthor(request.UpdatedAuthor);
            UpdateAuthorDetails(authorToUpdate, request.UpdatedAuthor);

            _authorRepository.Update(authorToUpdate);

            return Task.FromResult(authorToUpdate);
        }

        private void ValidateUpdatedAuthor(AuthorDto updatedAuthor)
        {
            if (string.IsNullOrWhiteSpace(updatedAuthor.Name))
            {
                throw new Exception("Author name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(updatedAuthor.BookCategory))
            {
                throw new Exception("Book category cannot be empty.");
            }
        }

        private void UpdateAuthorDetails(Author existingAuthor, AuthorDto updatedAuthor)
        {
            existingAuthor.Name = updatedAuthor.Name;
            existingAuthor.BookCategory = updatedAuthor.BookCategory;
        }

    }
}
