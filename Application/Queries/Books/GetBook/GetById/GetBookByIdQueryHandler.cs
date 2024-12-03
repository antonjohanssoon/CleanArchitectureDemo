using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Queries.Books.GetBook.GetById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, OperationResult<Book>>
    {
        private readonly IRepository<Book> _bookRepository;

        public GetBookByIdQueryHandler(IRepository<Book> repository)
        {
            _bookRepository = repository;
        }

        public Task<OperationResult<Book>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = _bookRepository.GetById(request.Id);

            if (book != null)
            {
                return Task.FromResult(OperationResult<Book>.Successfull(book));
            }

            return Task.FromResult(OperationResult<Book>.Failure($"Book with ID: {request.Id} was not found."));
        }
    }
}
