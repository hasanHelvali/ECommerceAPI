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
    public class FileReadRepository : ReadRepository<f.File>, IFileReadRepository
    {
        public FileReadRepository(ECommerceAPI_DBContext context) : base(context)
        {
        }
    }
}
