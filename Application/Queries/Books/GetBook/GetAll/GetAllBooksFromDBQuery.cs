using Domain;
using MediatR;

namespace Application.Queries.Books.GetBook.GetAll
{
    public class GetAllBooksFromDBQuery : IRequest<List<Book>>
    {

    }
}
