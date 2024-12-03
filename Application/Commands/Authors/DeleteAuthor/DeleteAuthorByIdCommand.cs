using Domain;
using MediatR;

namespace Application.Commands.Authors.DeleteAuthor
{
    public class DeleteAuthorByIdCommand : IRequest<OperationResult<Author>>
    {
        public DeleteAuthorByIdCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
