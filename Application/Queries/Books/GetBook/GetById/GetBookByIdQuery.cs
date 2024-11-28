using Domain;
using MediatR;

namespace Application.Queries.Books.GetBook.GetById
{
    public class GetBookByIdQuery : IRequest<Book>
    {
        public Guid Id { get; }

        public GetBookByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
