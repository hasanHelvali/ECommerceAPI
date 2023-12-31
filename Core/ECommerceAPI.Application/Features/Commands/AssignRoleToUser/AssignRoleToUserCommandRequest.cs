using MediatR;

namespace ECommerceAPI.Application.Features.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandRequest:IRequest<AssignRoleToUserCommandResponse>
    {
        public string UserId { get; set; }
        public string[] Roles{ get; set; }
    }
}