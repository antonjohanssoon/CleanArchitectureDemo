using Domain;
using MediatR;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookByIdCommand : IRequest<OperationResult<Book>>
    {
        public DeleteBookByIdCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
