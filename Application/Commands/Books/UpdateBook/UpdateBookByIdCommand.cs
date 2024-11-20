using Domain;
using MediatR;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookByIdCommand : IRequest<Book>
    {
        public UpdateBookByIdCommand(int id, Book updatedBook)
        {
            Id = id;
            UpdatedBook = updatedBook;
        }

        public int Id { get; }
        public Book UpdatedBook { get; }
    }
}