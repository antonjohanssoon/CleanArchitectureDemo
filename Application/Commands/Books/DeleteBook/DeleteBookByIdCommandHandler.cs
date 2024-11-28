using Application.Interfaces.RepositoryInterfaces;
using Domain;

using MediatR;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookByIdCommandHandler : IRequestHandler<DeleteBookByIdCommand, Book>
    {
        private readonly IRepository<Book> _bookRepository;

        public DeleteBookByIdCommandHandler(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<Book> Handle(DeleteBookByIdCommand request, CancellationToken cancellationToken)
        {
            Book bookToDelete = _bookRepository.GetById(request.Id);

            if (bookToDelete == null)
            {
                throw new Exception($"Book with ID: {request.Id} not found.");
            }

            _bookRepository.Delete(bookToDelete);
            return Task.FromResult(bookToDelete);
        }
    }
}
