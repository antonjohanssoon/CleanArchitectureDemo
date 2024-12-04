using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Books.GetBook.GetById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, OperationResult<Book>>
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly ILogger<GetBookByIdQueryHandler> _logger;

        public GetBookByIdQueryHandler(IRepository<Book> repository, ILogger<GetBookByIdQueryHandler> logger)
        {
            _bookRepository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<Book>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling request to fetch book with ID: {BookId}", request.Id);

                var book = _bookRepository.GetById(request.Id);

                if (book != null)
                {
                    _logger.LogInformation("Successfully found book with ID: {BookId}", request.Id);
                    return OperationResult<Book>.Successfull(book);
                }

                _logger.LogWarning("Book with ID: {BookId} not found.", request.Id);
                return OperationResult<Book>.Failure($"Book with ID: {request.Id} was not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the book with ID: {BookId}", request.Id);
                return OperationResult<Book>.Failure("An unexpected error occurred while fetching the book.");
            }
        }
    }
}


