using ECommerceAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandResponse
    {
    }
    public class LoginUserSuccessComandResponse : LoginUserCommandResponse
    {
        public Token Token { get; set; }
    }
    public class LoginUserErrorComandResponse : LoginUserCommandResponse
    {
        public string Message { get; set; }
    }
}
