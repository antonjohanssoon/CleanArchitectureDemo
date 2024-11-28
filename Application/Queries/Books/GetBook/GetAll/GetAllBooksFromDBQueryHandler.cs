using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Queries.Books.GetBook.GetAll
{
    public class GetAllBooksFromDBQueryHandler : IRequestHandler<GetAllBooksFromDBQuery, List<Book>>
    {
        private readonly IRepository<Book> _bookRepository;

        public GetAllBooksFromDBQueryHandler(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<List<Book>> Handle(GetAllBooksFromDBQuery request, CancellationToken cancellationToken)
        {
            var books = _bookRepository.GetAll().ToList();

            if (books.Count == 0)
            {
                throw new Exception($"Your list of books is empty");
            }

            return Task.FromResult(books);
        }
    }
}
