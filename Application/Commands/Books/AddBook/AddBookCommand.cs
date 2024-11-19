using Domain;
using MediatR;

namespace Application.Commands.Books.AddBook
{
    public class AddBookCommand : IRequest<Book>
    {
        public AddBookCommand(Book newBook)
        {
            NewBook = newBook;
        }

        public Book NewBook { get; }
    }
}
