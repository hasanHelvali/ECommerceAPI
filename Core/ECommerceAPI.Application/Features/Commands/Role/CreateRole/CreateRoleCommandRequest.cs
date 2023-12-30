using MediatR;

namespace ECommerceAPI.Application.Features.Commands.Role.CreateRole
{
    public class CreateRoleCommandRequest:IRequest<CreateRoleCommandResponse>
    {
        public string Name { get; set; }
    }
}