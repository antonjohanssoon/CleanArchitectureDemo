using Domain;
using MediatR;

namespace Application.Queries.Authors.GetAuthor.GetAll
{
    public class GetAllAuthorsFromDBQuery : IRequest<List<Author>>
    {
    }
}
