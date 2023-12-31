using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.AppUser.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
    {
        readonly IUserService _userService;

        public GetAllUsersQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async  Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
        {
            var users =  await  _userService.GetAllUsersAsync(request.Page, request.Size);
            return new GetAllUsersQueryResponse()
            {
                Users= users,
                TotalUsersCount= _userService.TotalUsersCount
            };
        }
    }
}
