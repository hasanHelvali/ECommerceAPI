using ECommerceAPI.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int minute)
        {
            Application.DTOs.Token token = new Application.DTOs.Token();

            //SecurityKey in simetrigini alıyoruz.
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //sifrelenmis kimligi olusturuyoruz.
            SigningCredentials signingCredentials = new(key, SecurityAlgorithms.HmacSha256);
            //SecurityKey burada sifrelenmis oldu.

            //Olusturulacak token ayarlarını veriyoruz.
            token.Expiration = DateTime.UtcNow.AddMinutes(minute);//Token in suresini belirledik.
            JwtSecurityToken securityToken = new(//Bir token olustur. Bu token in temel bilgilerini sana veriyorum.
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow,//Bu token uretildikten ne zaman sonra devreye girsin? Uretildigi anda devreye girsin demis oldum.
                                           //notBefore:DateTime.UtcNow.AddMinutes(1) dersek token 1 dakika sonra devreye girer.
                signingCredentials: signingCredentials//token i uretirken sifrelenmis olan secret key e gore uret.
                );

            //Token olusturucu sınıfından bir nesne alalım.
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(securityToken);//Token i ilgili konfigure edilen nesneye gore uretiyorum.
            //sifre ilgili Dto nesneme atandı.
            return token;
            //Token i urettik ve uretirken de dogrulamada kullanılacak verileri iceirisine gommuş olduk.
            //Artık bu TokenHandler service ini kullanan her yapı ilgili degerlere gore bir token elde edebilir.

            //Jwt de en kritik yer dogrularken ve olustururken token ile ilgili kullanılacak degerlerin aynı olması gerekir.
        }
    }
}
