using Domain;
using MediatR;

namespace Application.Commands.Authors.DeleteAuthor
{
    public class DeleteAuthorByIdCommand : IRequest<Author>
    {
        public DeleteAuthorByIdCommand(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
