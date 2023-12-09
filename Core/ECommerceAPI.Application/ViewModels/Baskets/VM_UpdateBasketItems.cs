using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.ViewModels.Baskets
{
    public class VM_UpdateBasketItems
    {
        public string BasketItemId { get; set; }
        public int Quantity { get; set; }
    }
}
