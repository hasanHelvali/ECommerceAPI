using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Persistance.Concretes;
using ECommerceAPI.Persistance.Contexts;
using ECommerceAPI.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {

            services.AddDbContext<ECommerceAPI_DBContext>(options => options.UseSqlServer(Configuration.ConnectionString),
                ServiceLifetime.Singleton);
            services.AddSingleton<ICustomerReadRepository,CustomerReadRepository>();//ICustomerRR istenince CustomerRR don.
            //Ilgili varlıkların davranıslarına gore singleton,scoped vs seklinde eklenebilir.
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            //Butun service ler bu sekilde eklenmis oldular.
        }
    }
}
