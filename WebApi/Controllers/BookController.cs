using Application.Commands.Books.AddBook;
using Application.Commands.Books.DeleteBook;
using Application.Commands.Books.UpdateBook;
using Application.Queries.GetBook;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator mediator;

        public BookController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: api/<BookController>
        [HttpGet]
        public Task<List<Book>> GetAllBooks()
        {
            return mediator.Send(new GetAllBooksFromDBQuery());
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public Task<Book> GetBookById(int bookId)
        {
            return mediator.Send(new GetBookByIdQuery(bookId));
        }

        // POST api/<BookController>
        [HttpPost]
        public async void Post([FromBody] Book bookToAdd)
        {
            await mediator.Send(new AddBookCommand(bookToAdd));
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public async Task<Book> Put(int id, [FromBody] Book bookToUpdate)
        {
            Book updatedBook = await mediator.Send(new UpdateBookByIdCommand(id, bookToUpdate.Title, bookToUpdate.Description));
            return updatedBook;
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public async void Delete(int bookId)
        {
            await mediator.Send(new DeleteBookByIdCommand(bookId));
        }
    }
}
