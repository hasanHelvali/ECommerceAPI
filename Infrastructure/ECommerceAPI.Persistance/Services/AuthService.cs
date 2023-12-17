using Azure.Core;
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.AppUser.LoginUser;
using ECommerceAPI.Application.Helpers;
using ECommerceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Services
{
    public class AuthService : IAuthService
    {
        readonly IConfiguration _configuration;
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        readonly IUserService _userService;
        readonly IMailService _mailService;

        public AuthService(IConfiguration configuration, UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService, IMailService mailService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _userService = userService;
            _mailService = mailService;
        }

        public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            string? audience = _configuration["ExternalLoginSettings:Google:Client_ID"];
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { audience }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, accessTokenLifeTime);
        }

        public async Task<Token> LoginAsync(string userNameOrEmail, string password, int accessTokenLifeTime)
        {
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(userNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(userNameOrEmail);
            }
            if (user == null)
                throw new NotFoundUserException();
            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (signInResult.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 15);
                return token;

            }
            throw new AuthenticationErrorException();
        }



        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(15, user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 300);
                return token;
            }
            else
                throw new NotFoundUserException();
        }

        async Task<Token> CreateUserExternalAsync(AppUser user, string Email, string Name, UserLoginInfo info, int accessTokenLifeTime)
        {
            bool result = user != null;

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(Email);
                if (user == null)
                {
                    user = new Domain.Entities.Identity.AppUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = Email,
                        UserName = Email,
                        NameSurname = Name,

                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }
            if (result)
            {
                await _userManager.AddLoginAsync(user, info);
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 300);
                return token;
            }
            throw new Exception("Invalid External Authentication");
        }
        public async Task PasswordResetAsync(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                //byte[] tokenBytes = Encoding.UTF8.GetBytes(resetToken);
                //resetToken = WebEncoders.Base64UrlEncode(tokenBytes);
                //Ilgili token i url de tasınabilecek bir formata almıs olduk.
                resetToken = resetToken.UrlEncode();//encode şslemi helper a tasındı.
                await _mailService.SendPasswordResetMailAsycn(user.Email, user.Id, resetToken);
                //user.Email yerine poarametreden alına mail adresi de gonderilebilirdi

            }
        }

        public async Task<bool> VerifyResetTokenAsync(string resetToket, string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToket);
                //resetToket = Encoding.UTF8.GetString(tokenBytes);
                resetToket = resetToket.UrlDecode();
                //decode islemi helper a tasındı.
                return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider,
                    "ResetPassword", resetToket);
            }
            return false;
        }
    }
}
