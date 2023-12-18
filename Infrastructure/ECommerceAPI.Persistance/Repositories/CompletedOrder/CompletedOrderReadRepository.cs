using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Repositories
{
    public class CompletedOrderReadRepository :ReadRepository<CompletedOrder>, ICompletedOrderReadRepository
    {
        public CompletedOrderReadRepository(ECommerceAPI_DBContext context) : base(context)
        {
        }

        public DbSet<CompletedOrder> Table => throw new NotImplementedException();

        public IQueryable<CompletedOrder> GetAll(bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<CompletedOrder> GetByIdAsync(string id, bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<CompletedOrder> GetSingleAsync(Expression<Func<CompletedOrder, bool>> method, bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CompletedOrder> GetWhere(Expression<Func<CompletedOrder, bool>> method, bool tracking = true)
        {
            throw new NotImplementedException();
        }
    }
}
