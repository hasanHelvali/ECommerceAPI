using ECommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Repositories
{
    public interface IOrderReadRepository<T>:IRepository<T> where T : BaseEntity
    {
        //Read den kasıt burada select sorgusu yapmaktir.

        IQueryable<T> GetAll();//Sorgu uzerinde calısacaksak IQueryable, inmemory de calısacaksak IEnumerable kullanırız.
        //Hangi turdeysek butun her seyi bana getir anlamına gelir.
        //List<> kullanmıyoruz. List IEnumerable dir. Datayi inmemory ye ceker ve onun uzerinde islem yapar. Buna dikkat ediyoruz.
        
        IQueryable<T> GetWhere(Expression<Func<T,bool>> method);
        //sarta gore getir.
        Task<T> GetSingleAsync(Expression<Func<T,bool>> method);
        //sarta uygun olan ilk nesneyi getir
        Task<T> GetByIdAsync(string id);
        //direkt id ile eslesen nesneyi getir.
    }
}
