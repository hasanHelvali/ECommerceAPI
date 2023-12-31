using Azure.Core;
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Application.Features.Commands.AppUser.CreateUser;
using ECommerceAPI.Application.Helpers;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
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



        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new NotFoundUserException();
        }
        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                resetToken = resetToken.UrlDecode();
                IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (result.Succeeded)
                    await _userManager.UpdateSecurityStampAsync(user);
                else
                    throw new PasswordChangeFailedException();
            }
        }

        public async Task<List<ListUser>> GetAllUsersAsync(int page, int size)
        {
            var users = await _userManager.Users
                  .Skip(page * size).Take(size)
                  .ToListAsync();
            return users.Select(user => new ListUser
            {
                Id = user.Id,
                Email = user.Email,
                NameSurname = user.NameSurname,
                TwoFactorEnabled = user.TwoFactorEnabled,
                UserName = user.UserName
            }).ToList();
        }

        public async Task AssignRoleToUserAsync(string userId, string[] roles)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user!=null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);  
                await _userManager.AddToRolesAsync(user, roles);
            }
        }

        public async Task<string[]> GetRolesToUserAsync(string userId)
        {
            AppUser user= await _userManager.FindByIdAsync(userId);
            if (user!=null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }
            return new string[] {} ;
        }

        public int TotalUsersCount => _userManager.Users.Count();

    }
}
