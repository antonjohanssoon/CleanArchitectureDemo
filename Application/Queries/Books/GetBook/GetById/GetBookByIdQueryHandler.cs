using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Queries.Books.GetBook.GetById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Book>
    {
        private readonly FakeDatabase fakeDatabase;

        public GetBookByIdQueryHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = fakeDatabase.Books.FirstOrDefault(book => book.Id == request.Id);

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID: {request.Id} was not found.");
            }

            return Task.FromResult(book);
        }
    }
}
