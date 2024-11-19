using Domain;
using MediatR;

namespace Application.Queries.GetBook
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
