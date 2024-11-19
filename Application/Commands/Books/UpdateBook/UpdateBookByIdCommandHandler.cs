using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookByIdCommandHandler : IRequestHandler<UpdateBookByIdCommand, Book>
    {
        private readonly FakeDatabase fakeDatabase;

        public UpdateBookByIdCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<Book> Handle(UpdateBookByIdCommand request, CancellationToken cancellationToken)
        {
            Book bookToUpdate = fakeDatabase.Books.FirstOrDefault(book => book.Id == request.Id);

            bookToUpdate.Title = request.Title;
            bookToUpdate.Description = request.Description;

            return Task.FromResult(bookToUpdate);
        }
    }
}
