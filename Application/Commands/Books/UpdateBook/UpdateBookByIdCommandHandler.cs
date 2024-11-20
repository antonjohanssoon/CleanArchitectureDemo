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
            Book bookToUpdate = fakeDatabase.Books.FirstOrDefault(book => book.Id == request.Id);

            if (bookToUpdate == null)
            {
                throw new Exception($"Book with ID: {request.Id} not found.");
            }

            if (string.IsNullOrWhiteSpace(request.UpdatedBook.Title))
            {
                throw new Exception("Author name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(request.UpdatedBook.Description))
            {
                throw new Exception("Book category cannot be empty.");
            }

            bookToUpdate.Title = request.UpdatedBook.Title;
            bookToUpdate.Description = request.UpdatedBook.Description;

            if (bookToUpdate.Title != request.UpdatedBook.Title || bookToUpdate.Description != request.UpdatedBook.Description)
            {
                throw new Exception("Failed to update book data.");
            }

            return Task.FromResult(bookToUpdate);
        }
    }
}
