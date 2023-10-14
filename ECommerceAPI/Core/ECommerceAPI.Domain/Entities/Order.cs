using ECommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public class Order:BaseEntity
    {
        /*
        BaseEntity den dolayi bir id buraya atanmak zorundadir. Eger biz buradaki id yi yonetmek istiyorsak eger 
        CustomerId seklinde bir prop taımı yani bir kolon tanımını burada yapmamiz gerekir.
        Bunu koymazsak ef core zaten kendisi boyle bir alan tutacak. Egerr koyarsak tuttugu alanın kontrolunu bize verip burasi 
        ile iliskilendirecek.
        */

        public Guid CustomerId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        public ICollection<Product> Products{ get; set; }//1 order da 1 den fazla product olabilir. 1-n iliski kurmus olduk.
        public Customer Customer { get; set; }
        //Bir order sadece bir customer a ait olabilir.
    }
}
