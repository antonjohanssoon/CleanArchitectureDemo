using Application.Dtos;
using Domain;
using MediatR;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorByIdCommand : IRequest<Author>
    {
        public UpdateAuthorByIdCommand(Guid id, AuthorDto updatedAuthor)
        {
            Id = id;
            UpdatedAuthor = updatedAuthor;
        }
        public Guid Id { get; set; }
        public AuthorDto UpdatedAuthor { get; set; }
    }
}
