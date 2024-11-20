using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Authors.UpdateAuthor
{
    public class UpdateAuthorByIdCommandHandler : IRequestHandler<UpdateAuthorByIdCommand, Author>
    {
        private readonly FakeDatabase fakeDatabase;

        public UpdateAuthorByIdCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<Author> Handle(UpdateAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            Author authorToUpdate = fakeDatabase.Authors.FirstOrDefault(author => author.Id == request.Id);

            authorToUpdate.Name = request.UpdatedAuthor.Name;
            authorToUpdate.BookCategory = request.UpdatedAuthor.BookCategory;

            return Task.FromResult(authorToUpdate);
        }
    }
}
