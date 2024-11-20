using Domain;
using MediatR;

namespace Application.Queries.Authors.GetAuthor.GetById
{
    public class GetAuthorByIdQuery : IRequest<Author>
    {
        public GetAuthorByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
