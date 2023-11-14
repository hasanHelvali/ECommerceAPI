﻿using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Infrastructure.Enums;
using ECommerceAPI.Infrastructure.Services;
using ECommerceAPI.Infrastructure.Services.Storage;
using ECommerceAPI.Infrastructure.Services.Storage.Local;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices (this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();

        }
        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T: class , IStorage
        {
            /*T eger IStorage dan turemis bir class ise eger bunu dogrula ve IStorage i talep eden yapıya gore buradaki T yi yani 
             class ı bana gonder.*/
            serviceCollection.AddScoped<IStorage, T>();
        }
        public static void AddStorage(this IServiceCollection serviceCollection,StorageType storageType) 
        {
            switch (storageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage,LocalStorage>();
                    break;
                case StorageType.Azure:
                    //serviceCollection.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    //serviceCollection.AddScoped<IStorage, AWSStorage>();
                    break;
                default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }

        //Inject mekanizmasını buradaki iki sekilde de duzenleyebiliriz.
    }
}