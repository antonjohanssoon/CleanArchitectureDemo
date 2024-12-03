using Application.Dtos;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookByIdCommandHandler : IRequestHandler<UpdateBookByIdCommand, OperationResult<Book>>
    {
        private readonly IRepository<Book> _bookRepository;

        public UpdateBookByIdCommandHandler(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<OperationResult<Book>> Handle(UpdateBookByIdCommand request, CancellationToken cancellationToken)
        {
            var bookToUpdate = _bookRepository.GetById(request.Id);
            if (bookToUpdate == null)
            {
                return Task.FromResult(OperationResult<Book>.Failure($"Book with ID: {request.Id} not found."));
            }

            var validationResult = ValidateUpdatedBook(request.UpdatedBook);
            if (!validationResult.IsSuccessfull)
            {
                return Task.FromResult(validationResult);
            }

            UpdateBookDetails(bookToUpdate, request.UpdatedBook);

            _bookRepository.Update(bookToUpdate);

            return Task.FromResult(OperationResult<Book>.Successfull(bookToUpdate, "Book updated successfully."));
        }

        private OperationResult<Book> ValidateUpdatedBook(BookDto updatedBook)
        {
            if (string.IsNullOrWhiteSpace(updatedBook.Title))
            {
                return OperationResult<Book>.Failure("Book title cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(updatedBook.Description))
            {
                return OperationResult<Book>.Failure("Book description cannot be empty.");
            }

            return OperationResult<Book>.Successfull(null);
        }

        private void UpdateBookDetails(Book existingBook, BookDto updatedBook)
        {
            existingBook.Title = updatedBook.Title;
            existingBook.Description = updatedBook.Description;
        }
    }

}
