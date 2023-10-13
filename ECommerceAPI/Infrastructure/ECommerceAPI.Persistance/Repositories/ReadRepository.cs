using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities.Common;
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
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ECommerceAPI_DBContext _dbContext;//IoC ye eklenmisti. Oradan gelecek.
        public ReadRepository(ECommerceAPI_DBContext context)
        {
            _dbContext = context;
            //DI Cercevesi uygulandi.
        }
        public DbSet<T> Table => _dbContext.Set<T>();//Burdadaki set bana DbSet turunden gerekli nesneyi dondurur.
        //Burada table uzerinden butun operasyonlari yapabiliriz.

        public IQueryable<T> GetAll(bool tracking = true)
        {
            //return Table;//dbset bir IQueryable dir.

            var query=Table.AsQueryable();
            if (!tracking)//eger tracking istenmiyorsa yani tracking degeri false geliyorsa
               query = query.AsNoTracking();//ilgili datanın takibini istemiyorum. 
            return query;
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        //=>Table.Where(method);
        {
            var query = Table.Where(method);
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        //=> await Table.FirstOrDefaultAsync(method);
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }
        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        //=> await Table.FindAsync(Guid.Parse(id));//Bu sekilde de bir kullanim yapılabilir.
        //=> await Table.FirstOrDefaultAsync(data => data.ID == Guid.Parse(id));
        /*Burada Id field i getirilebilmesi icin where T:class yapısını BaseEntity ye cektim.
         Cunku bana gelecek olan yapı bir id olabilir bir order vs olabilir farketmeksizin bu gelen gun sonunda bir 
        baseentity dir. 
        Bana da id field i lazım oldugundan dolayı ben t:class kısıtlayıcısını t:baseentity ye cekmis bulundum. 
        Bu aynı zamanda marker dessign pattern uygulamalarından biridir. Buradaki base entity turu de marker bir turdur. */

        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.ID == Guid.Parse(id));//IQuerable da calısıyorsak findasync yoktur. Bu yuzden marker uzerinden calısacaz.
        }

    }
}
