using Application.Interfaces.RepositoryInterfaces;
using Domain;

using MediatR;

namespace Application.Commands.Books.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, OperationResult<Book>>
    {
        private readonly IRepository<Book> _bookRepository;

        public AddBookCommandHandler(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<OperationResult<Book>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var validationResult = ValidateBook(request.NewBook);
            if (!validationResult.IsSuccessfull)
            {
                return Task.FromResult(validationResult);
            }

            _bookRepository.Add(request.NewBook);

            return Task.FromResult(OperationResult<Book>.Successfull(request.NewBook, "Book added successfully."));
        }

        private OperationResult<Book> ValidateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                return OperationResult<Book>.Failure("Book title is required and cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(book.Description))
            {
                return OperationResult<Book>.Failure("Book description is required and cannot be empty.");
            }

            var existingBookWithSameTitle = _bookRepository.GetAll()
                .Any(existingBook => existingBook.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase));
            if (existingBookWithSameTitle)
            {
                return OperationResult<Book>.Failure($"Book with title '{book.Title}' already exists.");
            }

            var existingBookWithSameDescription = _bookRepository.GetAll()
                .Any(existingBook => existingBook.Description.Equals(book.Description, StringComparison.OrdinalIgnoreCase));
            if (existingBookWithSameDescription)
            {
                return OperationResult<Book>.Failure($"Book with description '{book.Description}' already exists.");
            }

            return OperationResult<Book>.Successfull(null);
        }
    }

}
