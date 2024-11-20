using Domain;
using MediatR;

namespace Application.Queries.Books.GetBook.GetById
{
    public class GetBookByIdQuery : IRequest<Book>
    {
        public int Id { get; }

        public GetBookByIdQuery(int id)
        {
            Id = id;
        }
    }
}
