using Application.Commands.Authors.AddAuthor;
using Application.Commands.Authors.DeleteAuthor;
using Application.Commands.Authors.UpdateAuthor;
using Application.Queries.Authors.GetAuthor.GetAll;
using Application.Queries.Authors.GetAuthor.GetById;
using Domain;
using MediatR;

namespace Application
{
    public class AuthorMethods
    {
        private readonly IMediator mediator;

        public AuthorMethods(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task AddNewAuthor(Author author)
        {
            Author newAuthorToAdd = new Author(4, "Anton Johansson", "Self-biography");
            await mediator.Send(new AddAuthorCommand(newAuthorToAdd));
        }

        public async Task<List<Author>> GetAuthors()
        {
            var authors = await mediator.Send(new GetAllAuthorsFromDBQuery());
            return authors;
        }

        public async Task<Author> GetAuthorByID(int authorId)
        {
            var author = await mediator.Send(new GetAuthorByIdQuery(authorId));
            return author;
        }

        public async Task UpdateAuthor(int authorId, Author updatedAuthor)
        {
            await mediator.Send(new UpdateAuthorByIdCommand(authorId, updatedAuthor));

        }

        public async Task DeleteAuthor(int authorId)
        {
            await mediator.Send(new DeleteAuthorByIdCommand(authorId));
        }
    }
}
