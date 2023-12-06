using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public Application.DTOs.Token CreateAccessToken(int second,AppUser appUser)
        {
            Application.DTOs.Token token = new Application.DTOs.Token();

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            SigningCredentials signingCredentials = new(key, SecurityAlgorithms.HmacSha256);
            token.Expiration = DateTime.UtcNow.AddSeconds(second);
            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims:new List<Claim>
                {
                    new(ClaimTypes.Name,appUser.UserName)
                }
                );
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            //string refreshToken = CreateRefreshToken();
            //token.RefreshToken= refreshToken;
            
            token.RefreshToken = CreateRefreshToken();

            return token;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];//32 indekslik bir byte dizisi olusturuldu.

            //using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            //{
            //}
            /*Using in eski kullanmı bu sekildeydi. RandomNumberGenerator sınıfına gittigimiz zaman IDisposable dan turedigini goruruz.
             Yani yok edilebilir bir nesnedir. Bu tip nesnelerde bir using kullanabiliyoruz. using de ilgili nesne, kendi scope alanlarından 
            cıkana kadar bellekte durur ve kullanılır. Bu scope dan cıkınca da dispose yani imha edilir.*/

            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            /*Bu sekildeki kullanımda ise ilgili scope alanları metodun kendisidir. Compiler CreateRefreshToken fonksiyonunun
            scope alanlarından cıkıncaya kadar RandomNumberGenerator nesnesi dispose edilmeyecek. Buradaki using kullanımı ise bu 
            ise yarar.Burada scope alanını genisletmis oldum.*/
            random.GetBytes(number);
            //Ilgili byte dizisini random degerler ile dolduruyorum.
            return Convert.ToBase64String(number);
            //Burada ilgili byte degerlerini string e cekiyorum.

        }
    }
}
