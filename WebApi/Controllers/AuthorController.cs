﻿using Application.Commands.Authors.AddAuthor;
using Application.Commands.Authors.DeleteAuthor;
using Application.Commands.Authors.UpdateAuthor;
using Application.Queries.Authors.GetAuthor.GetAll;
using Application.Queries.Authors.GetAuthor.GetById;
using Domain;
using MediatR;
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
        [HttpGet]
        [Route("getAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            return Ok(await mediator.Send(new GetAllAuthorsFromDBQuery()));
        }

        // GET api/<AuthorController>/5
        [HttpGet]
        [Route("getAuthorById/{authorId}")]
        public async Task<IActionResult> GetAuthorById(int authorId)
        {
            return Ok(await mediator.Send(new GetAuthorByIdQuery(authorId)));
        }

        // POST api/<AuthorController>
        [HttpPost]
        [Route("addNewAuthor")]
        public async Task<IActionResult> AddNewAuthor([FromBody] Author newAuthor)
        {
            return Ok(await mediator.Send(new AddAuthorCommand(newAuthor)));
        }

        // PUT api/<AuthorController>/5
        [HttpPut]
        [Route("updateAuthor/{updatedAuthorId}")]
        public async Task<IActionResult> UpdateAuthor(int updatedAuthorId, [FromBody] Author updatedAuthor)
        {
            return Ok(await mediator.Send(new UpdateAuthorByIdCommand(updatedAuthorId, updatedAuthor)));
        }

        // DELETE api/<AuthorController>/5
        [HttpDelete]
        [Route("deleteAuthorById/{authorId}")]
        public async Task<IActionResult> DeleteAuthorById(int authorId)
        {
            return Ok(await mediator.Send(new DeleteAuthorByIdCommand(authorId)));
        }
    }
}
