using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Queries.Books.GetBook.GetById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Book>
    {
        private readonly IRepository<Book> _bookRepository;

        public GetBookByIdQueryHandler(IRepository<Book> repository)
        {
            _bookRepository = repository;
        }

        public Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = _bookRepository.GetById(request.Id);

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID: {request.Id} was not found.");
            }

            return Task.FromResult(book);
        }
    }
}
