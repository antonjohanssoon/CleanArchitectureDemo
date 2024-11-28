using Application.Dtos;
using Domain;
using MediatR;

namespace Application.Commands.Authors.AddAuthor
{
    public class AddAuthorCommand : IRequest<Author>
    {

        public AddAuthorCommand(AuthorDto newAuthor)
        {
            NewAuthor = newAuthor;
        }

        public AuthorDto NewAuthor { get; set; }
    }
}
