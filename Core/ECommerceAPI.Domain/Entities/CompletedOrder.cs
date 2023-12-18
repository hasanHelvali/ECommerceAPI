using ECommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public class CompletedOrder:BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
