using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Persistance.Contexts;
using ECommerceAPI.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.InvoiceFile
{
    internal class InvoiceFileWriteRepository : WriteRepository<ECommerceAPI.Domain.Entities.InvoiceFile>, IInvoiceFileWriteRepository
    {
        public InvoiceFileWriteRepository(ECommerceAPI_DBContext dBContext) : base(dBContext)
        {
        }
    }
}
