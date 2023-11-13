using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Repositories
{
    public class InvoiceFileReadRepository : ReadRepository<ECommerceAPI.Domain.Entities.InvoiceFile>, IInvioceFileReadRepository
    {
        public InvoiceFileReadRepository(ECommerceAPI_DBContext context) : base(context)
        {
        }
    }
}
