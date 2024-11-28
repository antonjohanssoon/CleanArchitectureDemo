using Application.Dtos;
using Domain;
using MediatR;

namespace Application.Commands.Books.AddBook
{
    public class AddBookCommand : IRequest<Book>
    {
        public AddBookCommand(BookDto newBook)
        {
            NewBook = newBook;
        }

        public BookDto NewBook { get; }
    }
}
