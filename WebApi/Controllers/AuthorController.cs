using Application.Commands.Authors.AddAuthor;
using Application.Commands.Authors.DeleteAuthor;
using Application.Commands.Authors.UpdateAuthor;
using Application.Dtos;
using Application.Queries.Authors.GetAuthor.GetAll;
using Application.Queries.Authors.GetAuthor.GetById;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        internal readonly IMediator mediator;

        public AuthorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: api/<AuthorController>
        [Authorize]
        [HttpGet]
        [Route("getAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var result = await mediator.Send(new GetAllAuthorsFromDBQuery());

            if (!result.IsSuccessfull)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            return Ok(new { message = result.Message, data = result.Data });
        }

        // GET api/<AuthorController>/5
        [HttpGet]
        [Route("getAuthorById/{authorId}")]
        public async Task<IActionResult> GetAuthorById(Guid authorId)
        {
            var query = new GetAuthorByIdQuery(authorId);
            var result = await mediator.Send(query);

            if (!result.IsSuccessfull)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            return Ok(new { message = result.Message, data = result.Data });
        }

        // POST api/<AuthorController>
        [HttpPost]
        [Route("addNewAuthor")]
        public async Task<IActionResult> AddNewAuthor([FromBody] Author newAuthor)
        {
            var command = new AddAuthorCommand(newAuthor);
            var result = await mediator.Send(command);

            if (!result.IsSuccessfull)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            return Ok(new { message = result.Message, data = result.Data });

        }

        // PUT api/<AuthorController>/5
        [HttpPut]
        [Route("updateAuthor/{authorId}")]
        public async Task<IActionResult> UpdateAuthor(Guid authorId, [FromBody] AuthorDto updatedAuthor)
        {
            var command = new UpdateAuthorByIdCommand(authorId, updatedAuthor);
            var result = await mediator.Send(command);

            if (!result.IsSuccessfull)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }

            return Ok(new { message = result.Message, data = result.Data });
        }


        // DELETE api/<AuthorController>/5
        [HttpDelete]
        [Route("deleteAuthorById/{authorId}")]
        public async Task<IActionResult> DeleteAuthorById(Guid authorId)
        {
            var command = new DeleteAuthorByIdCommand(authorId);
            var result = await mediator.Send(command);

            if (!result.IsSuccessfull)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            return Ok(new { message = result.Message, data = result.Data });
        }
    }
}
