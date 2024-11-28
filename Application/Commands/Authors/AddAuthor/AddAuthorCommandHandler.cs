using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Commands.Authors.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, Author>
    {
        private readonly IRepository<Author> _authorRepository;

        public AddAuthorCommandHandler(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<Author> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            ValidateAuthor(request.NewAuthor);
            CheckForDuplicateAuthor(request.NewAuthor);


            _authorRepository.Add(request.NewAuthor);

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
            var existingAuthors = _authorRepository.GetAll();
            if (existingAuthors.Any(existingAuthor =>
                existingAuthor.Name.Equals(author.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"Author with name '{author.Name}' already exists.");
            }
        }
    }
}
