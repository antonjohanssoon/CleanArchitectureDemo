using Application.Commands.Users.AddUser;
using Application.Queries.Users.GetAllUsers;
using Application.Queries.Users.Login;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        internal readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await mediator.Send(new GetAllUsersQuery());

            if (!result.IsSuccessfull)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpPost]
        [Route("addNewUser")]
        public async Task<IActionResult> RegisterNewUser([FromBody] User newUser)
        {
            var command = new AddUserCommand(newUser);
            var result = await mediator.Send(command);

            if (!result.IsSuccessfull)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            var query = new LoginUserQuery(loginUser);
            var result = await mediator.Send(query);

            if (!result.IsSuccessfull)
            {
                return Unauthorized(new { message = result.Message, errors = result.ErrorMessage });
            }

            return Ok(new { message = result.Message, token = result.Data });
        }

    }
}
