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

            if (authorToUpdate == null)
            {
                throw new Exception($"Author with ID: {request.Id} not found.");
            }

            if (string.IsNullOrWhiteSpace(request.UpdatedAuthor.Name))
            {
                throw new Exception("Author name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(request.UpdatedAuthor.BookCategory))
            {
                throw new Exception("Book category cannot be empty.");
            }

            authorToUpdate.Name = request.UpdatedAuthor.Name;
            authorToUpdate.BookCategory = request.UpdatedAuthor.BookCategory;

            if (authorToUpdate.Name != request.UpdatedAuthor.Name || authorToUpdate.BookCategory != request.UpdatedAuthor.BookCategory)
            {
                throw new Exception("Failed to update author data.");
            }

            return Task.FromResult(authorToUpdate);
        }
    }
}
