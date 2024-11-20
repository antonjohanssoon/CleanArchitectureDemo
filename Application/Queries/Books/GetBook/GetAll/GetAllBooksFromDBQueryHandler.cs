using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Queries.Books.GetBook.GetAll
{
    public class GetAllBooksFromDBQueryHandler : IRequestHandler<GetAllBooksFromDBQuery, List<Book>>
    {
        private readonly FakeDatabase fakeDatabase;

        public GetAllBooksFromDBQueryHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<List<Book>> Handle(GetAllBooksFromDBQuery request, CancellationToken cancellationToken)
        {
            if (fakeDatabase.Books.Count == 0)
            {
                throw new Exception($"Your list of books is empty");
            }

            return Task.FromResult(fakeDatabase.Books);
        }
    }
}
