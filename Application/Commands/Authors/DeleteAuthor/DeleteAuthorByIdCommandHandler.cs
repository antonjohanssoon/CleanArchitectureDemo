﻿using Domain;
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

            fakeDatabase.Authors.Remove(authorToDelete);
            return Task.FromResult(authorToDelete);
        }
    }
}
