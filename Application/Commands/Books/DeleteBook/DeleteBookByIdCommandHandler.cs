﻿using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Books.DeleteBook
{
    public class DeleteBookByIdCommandHandler : IRequestHandler<DeleteBookByIdCommand, Book>
    {
        private readonly FakeDatabase fakeDatabase;

        public DeleteBookByIdCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<Book> Handle(DeleteBookByIdCommand request, CancellationToken cancellationToken)
        {
            Book bookToDelete = fakeDatabase.Books.FirstOrDefault(book => book.Id == request.Id);

            fakeDatabase.Books.Remove(bookToDelete);
            return Task.FromResult(bookToDelete);
        }
    }
}
