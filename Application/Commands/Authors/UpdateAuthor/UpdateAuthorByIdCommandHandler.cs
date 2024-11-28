using Application.Dtos;
using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorByIdCommandHandler : IRequestHandler<UpdateAuthorByIdCommand, Author>
    {
        private readonly FakeDatabase fakeDatabase;

        public UpdateAuthorByIdCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }
        public Task<Author> Handle(UpdateAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            var authorToUpdate = GetAuthorById(request.Id);
            ValidateUpdatedAuthor(request.UpdatedAuthor);

            UpdateAuthorDetails(authorToUpdate, request.UpdatedAuthor);

            return Task.FromResult(authorToUpdate);
        }

        private Author GetAuthorById(Guid id)
        {
            var author = fakeDatabase.Authors.FirstOrDefault(author => author.Id == id);
            if (author == null)
            {
                throw new Exception($"Author with ID: {id} not found.");
            }
            return author;
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
