using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services.Authentications
{
    public interface IInternalAuthentication
    {
        Task<DTOs.Token> LoginAsync(string userNameOrEmail,string password,int accessTokenLifeTime);
        Task<DTOs.Token> RefreshTokenLoginAsync(string refreshToken);

        Task PasswordResetAsync(string email);
        Task<bool> VerifyResetTokenAsync(string resetToket, string userId);
    }
}
