using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Commands.Authors.DeleteAuthor
{
    public class DeleteAuthorByIdCommandHandler : IRequestHandler<DeleteAuthorByIdCommand, Author>
    {
        private readonly IRepository<Author> _authorRepository;

        public DeleteAuthorByIdCommandHandler(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<Author> Handle(DeleteAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            var authorToDelete = _authorRepository.GetById(request.Id);

            if (authorToDelete == null)
            {
                throw new Exception($"Author with ID: {request.Id} not found.");
            }

            _authorRepository.Delete(authorToDelete);
            return Task.FromResult(authorToDelete);
        }
    }
}
