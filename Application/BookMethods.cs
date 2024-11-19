using Application.Commands.Books.AddBook;
using Application.Commands.Books.DeleteBook;
using Application.Commands.Books.UpdateBook;
using Application.Queries.GetBook;
using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application
{
    public class BookMethods
    {
        private readonly FakeDatabase fakeDatabase;
        private readonly IMediator mediator;

        public BookMethods(FakeDatabase fakeDatabase, IMediator mediator)
        {
            this.fakeDatabase = fakeDatabase;
            this.mediator = mediator;
        }

        public async Task AddNewBook(Book book)
        {
            Book newBookToAdd = new Book(10, "Antons book", "A book about Antons life");
            await mediator.Send(new AddBookCommand(newBookToAdd));
        }

        public async Task<List<Book>> GetBooks()
        {
            var books = await mediator.Send(new GetAllBooksFromDBQuery());
            return books;
        }

        public async Task<Book> GetBookByID(int bookId)
        {
            var book = await mediator.Send(new GetBookByIdQuery(bookId));
            return book;
        }

        public async Task UpdateBook(int bookId)
        {
            await mediator.Send(new UpdateBookByIdCommand(bookId, "Nalle Puh", "A cute yellow bear"));

        }

        public async Task DeleteBook(int bookId)
        {
            await mediator.Send(new DeleteBookByIdCommand(bookId));
        }

    }
}
