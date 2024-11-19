using Domain;
using MediatR;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookByIdCommand : IRequest<Book>
    {
        public DeleteBookByIdCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
