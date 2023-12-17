using ECommerceAPI.Application.Abstractions.Services.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IAuthService:IInternalAuthentication,IExternalAuthentication
    {

        Task PasswordResetAsync(string email);
        Task<bool>VerifyResetTokenAsync(string resetToket,string userId);
    }
}
