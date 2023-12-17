using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUser.VerifyResfreshToken
{
    public class VerifyRefreshTokenCommandHandler : IRequestHandler<VerifyRefreshTokenCommandRequest, VerifyRefreshTokenCommandResponse>
    {
        readonly IAuthService _authService;

        public VerifyRefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<VerifyRefreshTokenCommandResponse> Handle(VerifyRefreshTokenCommandRequest request, CancellationToken cancellationToken)
        {
            bool state = await _authService.VerifyResetTokenAsync(request.ResetToken, request.UserId);
            return new()
            {
                State=state
            };

        }
    }
}
