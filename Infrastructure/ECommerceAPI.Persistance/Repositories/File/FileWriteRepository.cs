using f = ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Persistance.Contexts;

namespace ECommerceAPI.Persistance.Repositories
{
    public class FileWriteRepository : WriteRepository<f.File>, IFileWriteRepository
    {
        public FileWriteRepository(ECommerceAPI_DBContext dBContext) : base(dBContext)
        {
        }
    }
}
