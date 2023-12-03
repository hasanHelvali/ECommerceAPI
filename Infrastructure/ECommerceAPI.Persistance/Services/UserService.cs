using Azure.Core;
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Features.Commands.AppUser.CreateUser;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U = ECommerceAPI.Domain.Entities.Identity;
namespace ECommerceAPI.Persistance.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult identityResult = await _userManager.CreateAsync(new U.AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                NameSurname = model.NameSurname,
            }, model.Password);

            CreateUserResponse response = new CreateUserResponse()
            {
                Succeeded = identityResult.Succeeded,
            };
            if (identityResult.Succeeded)
                response.Message = "Kullanıcı Kaydı Başarıyla Yapıldı.";
            else
                foreach (var error in identityResult.Errors)
                    response.Message += $"{error.Code}-{error.Description}\n";
            
            return response;
        }
    }
}
