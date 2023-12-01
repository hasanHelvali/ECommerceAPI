using ECommerceAPI.Application.Features.Commands.AppUser.CreateUser;
using ECommerceAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.AppUser.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<string> selamla()
        {
            return "Selamun Aleykum";
        }

        [HttpPost]
        public async  Task<IActionResult> Create(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse createCommandRequestResponse = await _mediator.Send(createUserCommandRequest);
            return Ok(createCommandRequestResponse);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            LoginUserCommandResponse loginUserCommandResponse = await _mediator.Send(loginUserCommandRequest);
            return Ok(loginUserCommandResponse);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            GoogleLoginCommandResponse googleLoginCommandResponse = await _mediator.Send(googleLoginCommandRequest);
            //Gelen istek mediator ile handler a gonderildi. response elde edildi.

            return Ok(googleLoginCommandResponse);

        }
    }
}
