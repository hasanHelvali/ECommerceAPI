using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Features.Commands.AppUser.CreateUser;
using ECommerceAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.AppUser.LoginUser;
using ECommerceAPI.Application.Features.Commands.AppUser.UpdatePassword;
using ECommerceAPI.Application.Features.Commands.AssignRoleToUser;
using ECommerceAPI.Application.Features.Commands.AuthorizationEndpoints.AssignRoleEndpoint;
using ECommerceAPI.Application.Features.Queries.AppUser.GetAllUsers;
using ECommerceAPI.Application.Features.Queries.AppUser.GetRolesToUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse createCommandRequestResponse = await _mediator.Send(createUserCommandRequest);
            return Ok(createCommandRequestResponse);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest updatePasswordCommandRequest)
        {
            UpdatePasswordCommandResponse updatePasswordCommandResponse = await _mediator.Send(updatePasswordCommandRequest);
            return Ok(updatePasswordCommandResponse);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get All Users", Menu = "Users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQueryRequest getAllUsersQueryRequest)
        {
            GetAllUsersQueryResponse getAllUsersQueryResponse = await _mediator.Send(getAllUsersQueryRequest);
            return Ok(getAllUsersQueryResponse);
        }

        [HttpGet("get-roles-to-user/{UserId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles To User", Menu = "Users")]
        public async Task<IActionResult> GetRolesToUser([FromRoute] GetRolesToUserQueryRequest getRolesToUserQueryRequest)
        {
            GetRolesToUserQueryResponse getRolesToUserQueryResponse = await _mediator.Send(getRolesToUserQueryRequest);
            return Ok(getRolesToUserQueryResponse);
        }


        [HttpPost("assign-role-to-user")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Assign Role To User", Menu = "Users")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserCommandRequest assignRoleToUserCommandRequest)
        {
            AssignRoleToUserCommandResponse assignRoleToUserCommandResponse = await _mediator.Send(assignRoleToUserCommandRequest);
            return Ok(assignRoleToUserCommandResponse);
        }


    }
}
