using Domain;
using MediatR;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorByIdCommand : IRequest<Author>
    {
        public UpdateAuthorByIdCommand(int id, Author updatedAuthor)
        {
            Id = id;
            UpdatedAuthor = updatedAuthor;
        }
        public int Id { get; set; }
        public Author UpdatedAuthor { get; set; }
    }
}
