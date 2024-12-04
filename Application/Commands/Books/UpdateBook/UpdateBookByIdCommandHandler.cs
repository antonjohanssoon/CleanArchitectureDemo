using Application.Dtos;
using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookByIdCommandHandler : IRequestHandler<UpdateBookByIdCommand, OperationResult<Book>>
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly ILogger<UpdateBookByIdCommandHandler> _logger;

        public UpdateBookByIdCommandHandler(IRepository<Book> bookRepository, ILogger<UpdateBookByIdCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Book>> Handle(UpdateBookByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Attempting to update book with ID: {BookId}", request.Id);

                var bookToUpdate = _bookRepository.GetById(request.Id);
                if (bookToUpdate == null)
                {
                    _logger.LogWarning("Book with ID: {BookId} not found. Update failed.", request.Id);
                    return OperationResult<Book>.Failure($"Book with ID: {request.Id} not found.");
                }

                var validationResult = ValidateUpdatedBook(request.UpdatedBook);
                if (!validationResult.IsSuccessfull)
                {
                    _logger.LogWarning("Validation failed for book update with ID: {BookId}. Error: {ErrorMessage}", request.Id, validationResult.ErrorMessage);
                    return validationResult;
                }

                _logger.LogInformation("Updating book with ID: {BookId}. New Title: {NewTitle}, New Description: {NewDescription}",
                    request.Id, request.UpdatedBook.Title, request.UpdatedBook.Description);

                UpdateBookDetails(bookToUpdate, request.UpdatedBook);

                _bookRepository.Update(bookToUpdate);

                _logger.LogInformation("Book with ID: {BookId} updated successfully.", request.Id);

                return OperationResult<Book>.Successfull(bookToUpdate, "Book updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to update book with ID: {BookId}", request.Id);
                return OperationResult<Book>.Failure("An unexpected error occurred while updating the book.");
            }
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

