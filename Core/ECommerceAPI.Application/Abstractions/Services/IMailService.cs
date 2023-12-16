using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IMailService
    {
        Task SendMessageAsync(string to,string subject,string body,bool isBodyHtml=true);
        Task SendMessageAsync(string[] tos,string subject,string body,bool isBodyHtml=true);
        /*Bu sınıfın concrete ini herhangi bir db islemi yapmayacagımdan, tamamen dıs dunyaya hizmet verecek bir service olacagından dolayı 
        persistance da degil infrastructure da tasarlıyorum.*/
    }
}
