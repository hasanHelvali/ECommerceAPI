using ECommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public class Menu:BaseEntity
    {
        public string Name{ get; set; }
        public ICollection<Endpoint> Endpoints { get; set; }
    }
}
