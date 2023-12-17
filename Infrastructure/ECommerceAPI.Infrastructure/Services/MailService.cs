using ECommerceAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress(_configuration["Mail:UserName"], _configuration["Mail:Password"],System.Text.Encoding.UTF8);

            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:UserName"], _configuration["Mail:Password"]);
            smtp.Port =int.Parse(_configuration["Mail:Port"]);
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];
            await smtp.SendMailAsync(mail);

            //Ilk overload da buradaki algoritmayı kullanıyor.

        }

        public async Task SendPasswordResetMailAsycn(string to,string userId,string resetToken)
        {
            StringBuilder mail = new StringBuilder();
            mail.AppendLine("Merhaba <br> Eger yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz. <br>" +
                "<strong><a target=\"_blank\" href=\"");
            mail.AppendLine(_configuration["AngularCLientUrl"]);
            mail.AppendLine("/update-password/");
            mail.AppendLine(userId);
            mail.AppendLine("/");
            mail.AppendLine(resetToken);
            mail.AppendLine("\">Yeni Şifre Talebi İçin Tıklayınız...</a></strong><br><br><br><span style=\"font-size:12px;\" >Not:" +
                "Eger ki bu talep tarafınızca yapımamış ise lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla..." +
                "<br><br><br>HHS E-Ticaret");

            await SendMailAsync(to, "Şifre Yenileme Talebi", mail.ToString());
        }
    }
}
