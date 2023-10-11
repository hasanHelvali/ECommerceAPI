using ECommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Contexts
{
    public class ECommerceAPI_DBContext:DbContext
    {
        /*Verinin gelis yolu ile ilgili butun islemler persistance katmanında yapılmalidir. Db tarafını burada, 
         * code tarafında simule etmek istiyorsak eger bunu context class i ile yaparız. Bu context class i icin DBContext 
         class indan bir kalitim yapmamiz gerekmektedir. 
        Bu kalıtım icin Microsoft.EntiityFramework.Core paketine ihtiyac duyuyoruz.*/


        public ECommerceAPI_DBContext(DbContextOptions option):base(option)
        {
        /*IoC container a bu DBContext sınıfını vermemiz lazım. Bu yuzden ctor da ilgili islemleri yapmamiz gerekiyor.
         Ilgili options parametresi base e gonderiliyor. Bu ctor IoC da doldurulacak.*/

        }
        /*DBContext sınıfında ilgili entity lerr db ye aktarılmasi icin dbset olarak alınmalıdır.*/

        public DbSet<Product> Products{ get; set; }
        /*Db yi temsil eden DBContext yapısında Products adında Product tipinde veriler tutan bir tablo tutmus oldum.
        Yani Product sınıfının prop larinın kolon olarak tutuldugu bir tablo olusturulacagı anlamına geliyor.*/
        public DbSet<Order> Orders{ get; set; }
        public DbSet<Customer> Customers{ get; set; }


        /*Simdi bu yapıyı IoC container a eklemem lazım ki her yerden erisebileyim. 
         IoC container WEBApi projesinde yani presentation katmanında var. 
        Bunu bir seklilde oraya godnermem lazım. 
        Katmanlar arasında bir sekilde bir sey gonderirken Registration i kullanıyordum.
        Bunu oradan IoC ye ilgili datalari gonderiyorum. Oradaki yapı da zaten direkt olarak programcs kullanılıyor. */

    }
}
