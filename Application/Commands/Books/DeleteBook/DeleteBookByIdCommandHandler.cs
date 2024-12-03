using Application.Interfaces.RepositoryInterfaces;
using Domain;

using MediatR;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookByIdCommandHandler : IRequestHandler<DeleteBookByIdCommand, OperationResult<Book>>
    {
        private readonly IRepository<Book> _bookRepository;

        public DeleteBookByIdCommandHandler(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<OperationResult<Book>> Handle(DeleteBookByIdCommand request, CancellationToken cancellationToken)
        {
            Book bookToDelete = _bookRepository.GetById(request.Id);

            if (bookToDelete != null)
            {
                _bookRepository.Delete(bookToDelete);
                return Task.FromResult(OperationResult<Book>.Successfull(bookToDelete));
            }

            return Task.FromResult(OperationResult<Book>.Failure($"Book with ID: {request.Id} was not found."));
        }
    }
}
