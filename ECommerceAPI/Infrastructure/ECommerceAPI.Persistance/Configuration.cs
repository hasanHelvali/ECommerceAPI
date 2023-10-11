using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance
{
    static class Configuration
    {
        public static string ConnectionString
        {
            get 
            {
                ConfigurationManager configurationManager = new();//microsoft.extension.configuration kutuphanesi gerekir.
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ECommerceAPI.API"));//Microsoft.Extensions.Configuration.json kutuphanesi gerekir.
                configurationManager.AddJsonFile("appsettings.json");//Microsoft.Extensions.Configuration.json
                return configurationManager.GetConnectionString("SqlServerConnections");
            }
        }
    }
}
