using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Authors.DeleteAuthor
{
    public class DeleteAuthorByIdCommandHandler : IRequestHandler<DeleteAuthorByIdCommand, Author>
    {
        private readonly FakeDatabase fakeDatabase;

        public DeleteAuthorByIdCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<Author> Handle(DeleteAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            Author authorToDelete = fakeDatabase.Authors.FirstOrDefault(author => author.Id == request.Id);

            if (authorToDelete == null)
            {
                throw new Exception($"Author with ID: {request.Id} not found.");
            }

            fakeDatabase.Authors.Remove(authorToDelete);

            if (fakeDatabase.Authors.Contains(authorToDelete))
            {
                throw new Exception($"Failed to delete author with ID: {request.Id}.");
            }

            return Task.FromResult(authorToDelete);
        }
    }
}
