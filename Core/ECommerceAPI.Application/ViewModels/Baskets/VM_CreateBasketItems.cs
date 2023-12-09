using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.ViewModels.Baskets
{
    public class VM_CreateBasketItems
    {
        public string ProductId{ get; set; }
        public int Quantity{ get; set; }
    }
}
