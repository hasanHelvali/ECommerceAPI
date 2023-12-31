using MediatR;

namespace ECommerceAPI.Application.Features.Queries.AuthorizationEndpoints.GetRolesToEndpoints
{
    public class GetRolesToEndpointQueryRequest:IRequest<GetRolesToEndpointQueryResponse>
    {
        public string  Code { get; set; }
        public string  Menu{ get; set; }
    }
}