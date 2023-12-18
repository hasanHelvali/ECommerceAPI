using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Repositories
{
    public class CompletedOrderWriteRepository :WriteRepository<CompletedOrder>, ICompletedOrderWriteRepository
    {
        public CompletedOrderWriteRepository(ECommerceAPI_DBContext dBContext) : base(dBContext)
        {
        }

        public DbSet<CompletedOrder> Table => throw new NotImplementedException();

        public Task<bool> AddAsync(CompletedOrder model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddRangeAsync(List<CompletedOrder> datas)
        {
            throw new NotImplementedException();
        }

        public bool Remove(CompletedOrder model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(string id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRange(List<CompletedOrder> datas)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public bool Update(CompletedOrder model)
        {
            throw new NotImplementedException();
        }
    }
}
