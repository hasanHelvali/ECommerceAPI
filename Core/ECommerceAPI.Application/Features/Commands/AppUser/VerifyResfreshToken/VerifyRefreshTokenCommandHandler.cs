using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Services.Authentications;
using MediatR;

namespace ECommerceAPI.Application.Features.Commands.AppUser.VerifyResfreshToken
{
    public class VerifyRefreshTokenCommandHandler : IRequestHandler<VerifyRefreshTokenCommandRequest, VerifyRefreshTokenCommandResponse>
    {
        readonly IInternalAuthentication _authService;

        public VerifyRefreshTokenCommandHandler(IInternalAuthentication authService)
        {
            _authService = authService;
        }

        public async Task<VerifyRefreshTokenCommandResponse> Handle(VerifyRefreshTokenCommandRequest request, CancellationToken cancellationToken)
        {
            bool state = await _authService.VerifyResetTokenAsync(request.ResetToken, request.UserId);
            return new()
            {
                State = state
            };
        }
    }
    }
