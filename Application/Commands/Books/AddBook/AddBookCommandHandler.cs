using Application.Interfaces.RepositoryInterfaces;
using Domain;

using MediatR;

namespace Application.Commands.Books.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, Book>
    {
        private readonly IRepository<Book> _bookRepository;

        public AddBookCommandHandler(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<Book> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            ValidateBook(request.NewBook);

            _bookRepository.Add(request.NewBook);

            return Task.FromResult(request.NewBook);
        }

        private void ValidateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                throw new Exception("Book title is required and cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(book.Description))
            {
                throw new Exception("Book description is required and cannot be empty.");
            }

            var existingBookWithSameTitle = _bookRepository.GetAll();
            if (existingBookWithSameTitle.Any(existingBook =>
                existingBook.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"Book with title '{book.Title}' already exists.");
            }

            var existingBookWithSameDescription = _bookRepository.GetAll();
            if (existingBookWithSameDescription.Any(existingBook =>
                existingBook.Description.Equals(book.Description, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"Book with description '{book.Description}' already exists.");
            }
        }

    }
}
