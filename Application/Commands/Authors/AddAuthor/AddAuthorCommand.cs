using Domain;
using MediatR;

namespace Application.Commands.Authors.AddAuthor
{
    public class AddAuthorCommand : IRequest<Author>
    {

        public AddAuthorCommand(Author newAuthor)
        {
            NewAuthor = newAuthor;
        }

        public Author NewAuthor { get; set; }
    }
}
