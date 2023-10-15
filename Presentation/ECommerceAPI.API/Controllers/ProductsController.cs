using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository productWriteRepository;
        readonly private IProductReadRepository productReadRepository;

        private readonly IOrderWriteRepository orderWriteRepository;
        private readonly IOrderReadRepository orderReadRepository;
        private readonly ICustomerWriteRepository customerWriteRepository;
        //Burasi simdilik test amacli kullandıgım bir controller yapısı oldugu icin simdilik bu sekilde bir inject islemi yapömamız sorun olmaz.

        public ProductsController(IProductWriteRepository _productWriteRepository, IProductReadRepository _productReadRepository,
            ICustomerWriteRepository _customerWriteRepository, IOrderWriteRepository _orderWriteRepository, IOrderReadRepository orderReadRepository)//IoC talepleri yapıldı.
        {
            this.productWriteRepository = _productWriteRepository;
            this.productWriteRepository = _productWriteRepository;
            this.orderWriteRepository = _orderWriteRepository;
            this.customerWriteRepository = _customerWriteRepository;
            this.orderReadRepository = orderReadRepository;
        }


        [HttpGet]
        public async Task Get()
        {
            ////await productWriteRepository.AddAsync(new() { Name="A product",Price=1.500F,Stock=20,CreatedDate=DateTime.Now});
            //// await productWriteRepository.SaveAsync(); 

            //var customerId = Guid.NewGuid();

            /*id ben vermezsem kendisi verir. Lakin ben veriyorum ki burada customer ve order arasında bir iliski kurabileyim.
             Sonuc olarak customer ve order birbirini foreign key ile baglı. Bu yuzden once bir customer sonra bir order olusturuyorum.
            Bunlari kendi urettigim id ile de birbirlerine baglıyorum.*/

            //await customerWriteRepository.AddAsync(new Customer() {ID=customerId,Name="Hasan Helvali", });

            //await orderWriteRepository.AddAsync(new Order() {Description="Acıklama Yok.",Address="Bayburt Merkez",CustomerId=customerId,});
            //await orderWriteRepository.AddAsync(new Order() {Description="Acıklama Yok 2.",Address="Bayburt Gencosman",CustomerId=customerId});

            /*Goruldugu uzere CreatedDate ve updatedDate gibi alanlar bos bırakıldılar.
             Bu alanlar dbContext teki Interceptor mekanizması olarak belirledigimiz SaveChanges override inda doldurulacaklar.
            Artık her nesne olusturudugumda veya guncellemeye calıstıgımda bu alanlara deger vermeme gerek yoktur. Bunları bir
            yerden yometebiliyor durummdayiz.*/
            //await orderWriteRepository.SaveAsync();//Ilgili tetikleme burada baslatilacak. 
            //Ilgili kodlar isletildikten sonra yorum satırı haline getirilebilir.


            Order order = await orderReadRepository.GetByIdAsync("710FCE52-CCC3-4A5A-1C0E-08DBCCA336DD");
            //varolan bir veriyi elde ettik.
            order.Address = "Istanbul";//Bir deigisiklik yaptık.
            await orderWriteRepository.SaveAsync();//Ilgili tetikleme baslatıldı. 
            //Buradaki update islemi sonucunda updatedDate field inin dolduruldugunu gorebiliriz.
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(string id)
        //{
        //    Console.WriteLine(id);
        //    Product product= await productReadRepository.GetByIdAsync(id);
        //    return Ok(product);
        //}
    }
}
