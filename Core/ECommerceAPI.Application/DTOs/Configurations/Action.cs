using ECommerceAPI.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Configurations
{
    public class Action
    {
        public string ActionType{ get; set; }
        public string HttpType{ get; set; }
        public string Definition{ get; set; }
        public string Code { get; set; }// O an ki action ın kendisine ozel unique kodunu elde etmek istiyorum.
    }
}
