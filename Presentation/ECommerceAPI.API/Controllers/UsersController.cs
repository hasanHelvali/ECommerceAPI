using ECommerceAPI.Application.Features.Commands.AppUser.CreateUser;
using ECommerceAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.AppUser.LoginUser;
using ECommerceAPI.Application.Features.Commands.AppUser.UpdatePassword;
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
        [HttpPost]
        public async  Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse createCommandRequestResponse = await _mediator.Send(createUserCommandRequest);
            return Ok(createCommandRequestResponse);
        }
        
        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest updatePasswordCommandRequest)
        {
            UpdatePasswordCommandResponse updatePasswordCommandResponse=await _mediator.Send(updatePasswordCommandRequest);
            return Ok(updatePasswordCommandResponse);
        }


    }
}
