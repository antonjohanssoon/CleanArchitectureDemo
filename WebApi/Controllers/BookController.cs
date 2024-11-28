using Application.Commands.Books.AddBook;
using Application.Commands.Books.DeleteBook;
using Application.Commands.Books.UpdateBook;
using Application.Dtos;
using Application.Queries.Books.GetBook.GetAll;
using Application.Queries.Books.GetBook.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        internal readonly IMediator mediator;

        public BookController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: api/<BookController>
        [HttpGet]
        [Route("getAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await mediator.Send(new GetAllBooksFromDBQuery()));
        }

        // GET api/<BookController>/5
        [HttpGet]
        [Route("getBookById/{bookId}")]
        public async Task<IActionResult> GetBookById(Guid bookId)
        {
            return Ok(await mediator.Send(new GetBookByIdQuery(bookId)));
        }

        // POST api/<BookController>
        [HttpPost]
        [Route("addNewBook")]
        public async Task<IActionResult> AddNewBook([FromBody] BookDto newBook)
        {
            return Ok(await mediator.Send(new AddBookCommand(newBook)));
        }

        // PUT api/<BookController>/5
        [HttpPut]
        [Route("updateBook/{updatedBookId}")]
        public async Task<IActionResult> UpdateBook(Guid updatedBookId, [FromBody] BookDto updatedBook)
        {
            return Ok(await mediator.Send(new UpdateBookByIdCommand(updatedBookId, updatedBook)));
        }

        // DELETE api/<BookController>/5
        [HttpDelete]
        [Route("deleteBookById/{bookId}")]
        public async Task<IActionResult> DeleteBookById(Guid bookId)
        {
            return Ok(await mediator.Send(new DeleteBookByIdCommand(bookId)));
        }
    }
}
