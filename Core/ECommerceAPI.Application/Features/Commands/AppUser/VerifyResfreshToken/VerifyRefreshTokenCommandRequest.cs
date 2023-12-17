using MediatR;

namespace ECommerceAPI.Application.Features.Commands.AppUser.VerifyResfreshToken
{
    public class VerifyRefreshTokenCommandRequest:IRequest<VerifyRefreshTokenCommandResponse>
    {
        public string ResetToken { get; set; }
        public string UserId { get; set; }
    }
}