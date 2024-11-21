using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookByIdCommandHandler : IRequestHandler<UpdateBookByIdCommand, Book>
    {
        private readonly FakeDatabase fakeDatabase;

        public UpdateBookByIdCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }
        public Task<Book> Handle(UpdateBookByIdCommand request, CancellationToken cancellationToken)
        {
            var bookToUpdate = GetBookById(request.Id);
            ValidateUpdatedBook(request.UpdatedBook);

            UpdateBookDetails(bookToUpdate, request.UpdatedBook);

            return Task.FromResult(bookToUpdate);
        }

        private Book GetBookById(int id)
        {
            var book = fakeDatabase.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                throw new Exception($"Book with ID: {id} not found.");
            }
            return book;
        }

        private void ValidateUpdatedBook(Book updatedBook)
        {
            if (string.IsNullOrWhiteSpace(updatedBook.Title))
            {
                throw new Exception("Book title cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(updatedBook.Description))
            {
                throw new Exception("Book description cannot be empty.");
            }
        }

        private void UpdateBookDetails(Book existingBook, Book updatedBook)
        {
            existingBook.Title = updatedBook.Title;
            existingBook.Description = updatedBook.Description;
        }

    }
}
