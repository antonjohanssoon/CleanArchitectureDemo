using Domain;
using MediatR;

namespace Application.Queries.Authors.GetAuthor.GetById
{
    public class GetAuthorByIdQuery : IRequest<Author>
    {
        public GetAuthorByIdQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
