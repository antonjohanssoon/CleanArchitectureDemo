using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Queries.Books.GetBook.GetAll
{
    public class GetAllBooksFromDBQueryHandler : IRequestHandler<GetAllBooksFromDBQuery, OperationResult<List<Book>>>
    {
        private readonly IRepository<Book> _bookRepository;

        public GetAllBooksFromDBQueryHandler(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<OperationResult<List<Book>>> Handle(GetAllBooksFromDBQuery request, CancellationToken cancellationToken)
        {
            var books = _bookRepository.GetAll().ToList();

            if (books.Any())
            {
                return Task.FromResult(OperationResult<List<Book>>.Successfull(books));
            }

            return Task.FromResult(OperationResult<List<Book>>.Failure($"Your list of books are empty..."));
        }
    }
}
