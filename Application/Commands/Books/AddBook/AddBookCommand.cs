using Application.Dtos;
using Domain;
using MediatR;

namespace Application.Commands.Books.AddBook
{
    public class AddBookCommand : IRequest<OperationResult<Book>>
    {
        private BookDto bookDto;

        public AddBookCommand(Book newBook)
        {
            NewBook = newBook;
        }

        public AddBookCommand(BookDto bookDto)
        {
            this.bookDto = bookDto;
        }

        public Book NewBook { get; }
    }
}
