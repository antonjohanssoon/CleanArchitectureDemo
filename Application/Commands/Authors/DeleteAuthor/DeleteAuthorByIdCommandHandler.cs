using Application.Interfaces.RepositoryInterfaces;
using Domain;
using MediatR;

namespace Application.Commands.Authors.DeleteAuthor
{
    public class DeleteAuthorByIdCommandHandler : IRequestHandler<DeleteAuthorByIdCommand, OperationResult<Author>>
    {
        private readonly IRepository<Author> _authorRepository;

        public DeleteAuthorByIdCommandHandler(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<OperationResult<Author>> Handle(DeleteAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            var authorToDelete = _authorRepository.GetById(request.Id);

            if (authorToDelete != null)
            {
                _authorRepository.Delete(authorToDelete);
                return Task.FromResult(OperationResult<Author>.Successfull(authorToDelete));
            }

            return Task.FromResult(OperationResult<Author>.Failure($"Author with ID: {request.Id} was not found."));
        }
    }
}
