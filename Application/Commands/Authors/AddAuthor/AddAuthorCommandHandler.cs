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


            fakeDatabase.Authors.Add(request.NewAuthor);

            return Task.FromResult(request.NewAuthor);
        }

        private void ValidateAuthor(Author author)
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

        private void CheckForDuplicateAuthor(Author author)
        {
            if (fakeDatabase.Authors.Any(existingAuthor => existingAuthor.Name.Equals(author.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"Author with name '{author.Name}' already exists.");
            }
        }
    }
}
