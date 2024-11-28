using Domain;
using MediatR;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookByIdCommand : IRequest<Book>
    {
        public DeleteBookByIdCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
