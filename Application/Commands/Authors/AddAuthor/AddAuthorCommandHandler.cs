using Application.Dtos;
using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Authors.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, Author>
    {
        private readonly FakeDatabase fakeDatabase;

        public AddAuthorCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<Author> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            ValidateAuthor(request.NewAuthor);
            CheckForDuplicateAuthor(request.NewAuthor);

            var newAuthor = new Author(request.NewAuthor.Name, request.NewAuthor.BookCategory);
            fakeDatabase.Authors.Add(newAuthor);

            return Task.FromResult(newAuthor);
        }

        private void ValidateAuthor(AuthorDto author)
        {

            if (string.IsNullOrWhiteSpace(author.Name))
            {
                throw new Exception("Author name is required and cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(author.BookCategory))
            {
                throw new Exception("Author book category is required and cannot be empty.");
            }
        }

        private void CheckForDuplicateAuthor(AuthorDto author)
        {
            if (fakeDatabase.Authors.Any(existingAuthor => existingAuthor.Name.Equals(author.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"Author with name '{author.Name}' already exists.");
            }
        }
    }
}
