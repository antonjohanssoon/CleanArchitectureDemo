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
            fakeDatabase.Books.Add(request.NewBook);

            return Task.FromResult(request.NewBook);
        }
    }
}
