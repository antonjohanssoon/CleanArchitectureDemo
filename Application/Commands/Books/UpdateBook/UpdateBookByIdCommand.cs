using Application.Dtos;
using Domain;
using MediatR;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookByIdCommand : IRequest<Book>
    {
        public UpdateBookByIdCommand(Guid id, BookDto updatedBook)
        {
            Id = id;
            UpdatedBook = updatedBook;
        }

        public Guid Id { get; }
        public BookDto UpdatedBook { get; }
    }
}