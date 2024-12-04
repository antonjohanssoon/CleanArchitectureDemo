using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Books.AddBook
{

    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, OperationResult<Book>>
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly ILogger<AddBookCommandHandler> _logger;

        public AddBookCommandHandler(IRepository<Book> bookRepository, ILogger<AddBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Book>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = ValidateBook(request.NewBook);
                if (!validationResult.IsSuccessfull)
                {
                    _logger.LogWarning("Validation failed for book: {Title}", request.NewBook.Title);
                    return validationResult;
                }

                _logger.LogInformation("Adding new book: {Title}", request.NewBook.Title);

                await _bookRepository.Add(request.NewBook);

                return OperationResult<Book>.Successfull(request.NewBook, "Book added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the book.");
                return OperationResult<Book>.Failure("An unexpected error occurred while adding the book.");
            }
        }

        private OperationResult<Book> ValidateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                return OperationResult<Book>.Failure("Book title is required.");
            }

            if (string.IsNullOrWhiteSpace(book.Description))
            {
                return OperationResult<Book>.Failure("Book description is required.");
            }

            return OperationResult<Book>.Successfull(book);
        }
    }
}
