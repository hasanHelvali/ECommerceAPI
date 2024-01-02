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
                try
                {
                    /*Development ortamınd aburadaki dizin okuması basarılı olur. Deploy edilince uygulama tek bir yapı altında toplanır ve buradaki dosyalar
                    okunamaz. Bu durumda da catch e duseriz. Deploy icin okumayı orada yapalım.*/
                    configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ECommerceAPI.API"));//Microsoft.Extensions.Configuration.json kutuphanesi gerekir.
                    configurationManager.AddJsonFile("appsettings.json");//Microsoft.Extensions.Configuration.json
                }
                catch (Exception)
                {
                    //Deploy ortamında ise okuma buradan yapılabilir.

                    configurationManager.AddJsonFile("appsettings.Production.json");
                }
                return configurationManager.GetConnectionString("SqlServerConnections");
            }
        }
    }
}
