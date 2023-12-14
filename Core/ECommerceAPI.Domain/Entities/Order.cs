using ECommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public Basket Basket { get; set; }
        public string OrderCode { get; set; }
        //Her ordr olusturulurken bir kod uretilecek ve uretilen bu kod siparis sureclerinde kullanılacak.
    }
}
