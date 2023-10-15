using ECommerceAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity //generic ture bir constraint vermezsek hata alırız.
    {
        //Butun repository lerin icerisinde olmasını istedigim seyleri burada bulundurabilirim.
        DbSet<T> Table { get; }


    }
}
