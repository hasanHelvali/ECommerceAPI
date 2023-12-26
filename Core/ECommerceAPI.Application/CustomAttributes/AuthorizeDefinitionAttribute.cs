using ECommerceAPI.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.CustomAttributes
{
    public class AuthorizeDefinitionAttribute:Attribute
    {
        //Ilgili action i bir attribute ile isaretleyip bu isaretlenenn yapının bir rol tabanlı yetkilendirmeye tabii tutulmasını istiyorum.
        public string Menu{ get; set; }
        public string Definition{ get; set; }
        public ActionType ActionType{ get; set; }
    }
}
