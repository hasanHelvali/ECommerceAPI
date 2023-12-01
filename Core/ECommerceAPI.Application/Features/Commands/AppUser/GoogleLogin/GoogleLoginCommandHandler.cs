using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;

        public GoogleLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { "958479602575-v6j54rnag5khs34cmgfgpf2q3kulpnv8.apps.googleusercontent.com" }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
            var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider,info.ProviderKey );

            bool result = user != null;

            if (user==null)//dıs kaynaklı login lerin oldugu tabloda yoksa
            {
                user = await _userManager.FindByEmailAsync(payload.Email);//ne olur ne olmaz manuel olarak kaydolanlar arasında da bu email de biri yoksa 
                if (user==null)
                {
                    //Bu kullanıcı kesinlikle benim veri tabanımda yoksa
                    user=new Domain.Entities.Identity.AppUser()
                    {
                        Id=Guid.NewGuid().ToString(),
                        Email=payload.Email,
                        UserName=payload.Email,
                        NameSurname=payload.Name,

                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }
            if (result)
                await _userManager.AddLoginAsync(user, info);//AspNetUserLogins Tablsuna ilgili kullanıcı eklenmis oldu
            else
                throw new Exception("Invalid External Authentication");


            Token token = _tokenHandler.CreateAccessToken(5);
            return new GoogleLoginCommandResponse
            {
                Token = token
            }; 




        }
    }
}
