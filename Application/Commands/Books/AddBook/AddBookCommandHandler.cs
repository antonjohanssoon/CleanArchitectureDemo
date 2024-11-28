﻿using Application.Dtos;
using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Books.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, Book>
    {
        private readonly FakeDatabase fakeDatabase;

        public AddBookCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<Book> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            ValidateBook(request.NewBook);

            var newBook = new Book(request.NewBook.Title, request.NewBook.Description);
            fakeDatabase.Books.Add(newBook);

            return Task.FromResult(newBook);
        }

        private void ValidateBook(BookDto book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                throw new Exception("Book title is required and cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(book.Description))
            {
                throw new Exception("Book description is required and cannot be empty.");
            }

            var existingBookWithSameTitle = fakeDatabase.Books.FirstOrDefault(b => b.Title == book.Title);
            if (existingBookWithSameTitle != null)
            {
                throw new Exception($"Book with title '{book.Title}' already exists.");
            }

            var existingBookWithSameDescription = fakeDatabase.Books.FirstOrDefault(b => b.Description == book.Description);
            if (existingBookWithSameDescription != null)
            {
                throw new Exception($"Book with description '{book.Description}' already exists.");
            }
        }

    }
}
