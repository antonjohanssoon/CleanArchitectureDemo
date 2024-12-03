using Application.Commands.Books.AddBook;
using Application.Commands.Books.DeleteBook;
using Application.Commands.Books.UpdateBook;
using Application.Dtos;
using Application.Queries.Books.GetBook.GetAll;
using Application.Queries.Books.GetBook.GetById;
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
            var result = await mediator.Send(new GetAllBooksFromDBQuery());

            if (!result.IsSuccessfull)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            return Ok(new { message = result.Message, data = result.Data });
        }

        // GET api/<BookController>/5
        [HttpGet]
        [Route("getBookById/{bookId}")]
        public async Task<IActionResult> GetBookById(Guid bookId)
        {
            var query = new GetBookByIdQuery(bookId);
            var result = await mediator.Send(query);

            if (!result.IsSuccessfull)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            return Ok(new { message = result.Message, data = result.Data });
        }

        // POST api/<BookController>
        [HttpPost]
        [Route("addNewBook")]
        public async Task<IActionResult> AddNewBook([FromBody] Book newBook)
        {
            var command = new AddBookCommand(newBook);
            var result = await mediator.Send(command);

            if (!result.IsSuccessfull)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            return Ok(new { message = result.Message, data = result.Data });
        }

        // PUT api/<BookController>/5
        [HttpPut]
        [Route("updateBook/{updatedBookId}")]
        public async Task<IActionResult> UpdateBook(Guid bookId, [FromBody] BookDto updatedBook)
        {
            var command = new UpdateBookByIdCommand(bookId, updatedBook);
            var result = await mediator.Send(command);

            if (!result.IsSuccessfull)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            return Ok(new { message = result.Message, data = result.Data });
        }

        // DELETE api/<BookController>/5
        [HttpDelete]
        [Route("deleteBookById/{bookId}")]
        public async Task<IActionResult> DeleteBookById(Guid bookId)
        {
            var command = new DeleteBookByIdCommand(bookId);
            var result = await mediator.Send(command);

            if (!result.IsSuccessfull)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            return Ok(new { message = result.Message, data = result.Data });
        }
    }
}
