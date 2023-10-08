﻿using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Persistance.Concretes;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton<IProductService, ProductService>();
            //IProductService talebi istendiginde ProductService geri gonder.
            //Persistance da onlarca service ve sınıf olabilir. Bunlari IoC ye ekleyebilmem icin buradaki extension metot dan yararlanacam. 
            //Bu extension metot IoC container bulunduran WebApi projesinde tetiklenmesi gerekiyor.
        }
    }
}
