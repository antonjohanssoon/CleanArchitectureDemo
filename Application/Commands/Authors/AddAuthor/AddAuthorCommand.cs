using Application.Dtos;
using Domain;
using MediatR;

namespace Application.Commands.Authors.AddAuthor
{
    public class AddAuthorCommand : IRequest<OperationResult<Author>>
    {
        private AuthorDto authorDto;

        public AddAuthorCommand(Author newAuthor)
        {
            NewAuthor = newAuthor;
        }

        public AddAuthorCommand(AuthorDto authorDto, Author newAuthor)
        {
            this.authorDto = authorDto;
        }

        public Author NewAuthor { get; set; }
    }
}
