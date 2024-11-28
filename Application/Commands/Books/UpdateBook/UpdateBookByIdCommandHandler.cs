using Application.Dtos;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookByIdCommandHandler : IRequestHandler<UpdateBookByIdCommand, Book>
    {
        private readonly IRepository<Book> _bookRepository;

        public UpdateBookByIdCommandHandler(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<Book> Handle(UpdateBookByIdCommand request, CancellationToken cancellationToken)
        {
            var bookToUpdate = _bookRepository.GetById(request.Id);
            if (bookToUpdate == null)
            {
                throw new Exception($"Book with ID: {request.Id} not found.");
            }

            ValidateUpdatedBook(request.UpdatedBook);

            UpdateBookDetails(bookToUpdate, request.UpdatedBook);

            _bookRepository.Update(bookToUpdate);

            return Task.FromResult(bookToUpdate);
        }

        private void ValidateUpdatedBook(BookDto updatedBook)
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

        private void UpdateBookDetails(Book existingBook, BookDto updatedBook)
        {
            existingBook.Title = updatedBook.Title;
            existingBook.Description = updatedBook.Description;
        }

    }
}
