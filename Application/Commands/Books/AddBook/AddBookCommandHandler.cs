using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Books.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, Book>
    {
        private readonly FakeDatabase fakeDatabase;

        public AddBookCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<Book> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            ValidateBook(request.NewBook);
            CheckForDuplicateBookId(request.NewBook);

            fakeDatabase.Books.Add(request.NewBook);

            return Task.FromResult(request.NewBook);
        }

        private void ValidateBook(Book book)
        {
            if (book.Id <= 0)
            {
                throw new Exception("Book ID must be greater than 0 and cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(book.Title))
            {
                throw new Exception("Book title is required and cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(book.Description))
            {
                throw new Exception("Book description is required and cannot be empty.");
            }
        }

        private void CheckForDuplicateBookId(Book book)
        {
            if (fakeDatabase.Books.Any(existingBook => existingBook.Id == book.Id))
            {
                throw new Exception($"Book with ID '{book.Id}' already exists.");
            }
        }
    }
}
