using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Common;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Contexts
{
    public class ECommerceAPI_DBContext:IdentityDbContext<AppUser,AppRole,string>
    {

        public ECommerceAPI_DBContext(DbContextOptions option):base(option)
        {

        }

        public DbSet<Product> Products{ get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<Customer> Customers{ get; set; }
        public DbSet<Domain.Entities.File> Files { get; set; }
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<ECommerceAPI.Domain.Entities.InvoiceFile> invoiceFiles { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            /*SaveChanges tetiklendiginde ilgili datalar uzerinde bazı degisiklikler yapabilirim.
             Bunu burada araya girerek yaptıgım icin bu mekanizmaya interceptor diyoruz. */

            var datas = ChangeTracker.Entries<BaseEntity>(); //Ilgili degisikligi takip eden property dir.
                                                            //Bundan sonra surece giren butun base entity ler yakalanırlar. Burada baseentity yi secmemin nedeni butun dataların ozunde bir baseentity olmasıdır.

            foreach (var data in datas)
            {
                _ = data.State switch//_ kullanmamın nedeni burada bir atama yapmak istemememdir. Buna discard yapısı denir.
                {
                    EntityState.Added=>data.Entity.CreatedDate=DateTime.Now,//Eger gelen data eklemeyle gelmisse createdDate eklenir.
                    EntityState.Modified=> data.Entity.UpdatedDate = DateTime.Now,////Eger gelen data guncellemeyle gelmisse updatedDate eklenir.
                    _ =>DateTime.UtcNow 
                };
            }
            return await base.SaveChangesAsync(cancellationToken);//SaveChangesAsync fonksiyonunu tekrardan deverye sokuyorum.

            /*Bir veriyi eklerken veya bir veriyi guncellerken ne zaman saveChangesAsync i tetiklersek once buradaki override tetiklenecek.
             Ilgılı yakalama ve manipulasyon islemleri yurutulecek.
            Sonra savechanges tektardan return de cagrılıp, son degisiklige gore tekrardan calıstırılacak.*/
        }
    }
}
