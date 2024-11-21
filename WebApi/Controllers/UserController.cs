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
            return Ok(await mediator.Send(new GetAllUsersQuery()));
        }

        [HttpPost]
        [Route("addNewUser")]
        public async Task<IActionResult> RegisterNewUser([FromBody] User newUser)
        {
            return Ok(await mediator.Send(new AddUserCommand(newUser)));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser([FromBody] User userLoggingIn)
        {
            return Ok(await mediator.Send(new LoginUserQuery(userLoggingIn)));
        }
    }
}
