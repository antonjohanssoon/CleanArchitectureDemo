using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookByIdCommandHandler : IRequestHandler<DeleteBookByIdCommand, OperationResult<Book>>
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly ILogger<DeleteBookByIdCommandHandler> _logger;

        public DeleteBookByIdCommandHandler(IRepository<Book> bookRepository, ILogger<DeleteBookByIdCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Book>> Handle(DeleteBookByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Attempting to delete book with ID: {BookId}", request.Id);

                Book bookToDelete = _bookRepository.GetById(request.Id);

                if (bookToDelete != null)
                {
                    _bookRepository.Delete(bookToDelete);
                    _logger.LogInformation("Book with ID: {BookId} deleted successfully.", request.Id);
                    return OperationResult<Book>.Successfull(bookToDelete, "Book deleted successfully.");
                }

                _logger.LogWarning("Book with ID: {BookId} was not found. Deletion failed.", request.Id);
                return OperationResult<Book>.Failure($"Book with ID: {request.Id} was not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to delete book with ID: {BookId}", request.Id);
                return OperationResult<Book>.Failure("An unexpected error occurred while deleting the book.");
            }
        }
    }
}

