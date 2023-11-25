using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> manager,
            SignInManager<Domain.Entities.Identity.AppUser> signInManager,
            ITokenHandler tokenHandler)
        {
            _userManager = manager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            }
            if (user == null)
                throw new NotFoundUserException();
            //Artık bir user bize kesinlikle getiriliyorsa 
            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (signInResult.Succeeded)//Authentication basarili olmus demektir.
            {
                //Yetkilerin Burada belirlenmesi gerekir. Yani olusturulan bir token sınıfını cagırıyor olalım.
                DTOs.Token token = _tokenHandler.CreateAccessToken(5);//5dk lik bir access token olusturmus oldum. Bunu da token nesnesi uzerinde tasıyorum.
                return new LoginUserSuccessComandResponse{ 
                Token=token
                };
            }
            //return new LoginUserErrorComandResponse
            //{
            //    Message = "Kullanıcı Adı veya Şifre Hatalı...!"
            //};
            throw new AuthenticationErrorException(); 
            //Ikı sekilde de donus yapabiliriz.
        }
    }
}
