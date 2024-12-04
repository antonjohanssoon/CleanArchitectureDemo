using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Books.GetBook.GetAll
{
    public class GetAllBooksFromDBQueryHandler : IRequestHandler<GetAllBooksFromDBQuery, OperationResult<List<Book>>>
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly ILogger<GetAllBooksFromDBQueryHandler> _logger;

        public GetAllBooksFromDBQueryHandler(IRepository<Book> bookRepository, ILogger<GetAllBooksFromDBQueryHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<Book>>> Handle(GetAllBooksFromDBQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling request to fetch all books from the database.");

                var books = _bookRepository.GetAll().ToList();

                if (books.Any())
                {
                    _logger.LogInformation("Successfully retrieved {BookCount} books from the database.", books.Count);
                    return OperationResult<List<Book>>.Successfull(books);
                }

                _logger.LogWarning("No books found in the database.");
                return OperationResult<List<Book>>.Failure("Your list of books is empty...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching books from the database.");
                return OperationResult<List<Book>>.Failure("An unexpected error occurred while fetching books.");
            }
        }
    }
}

