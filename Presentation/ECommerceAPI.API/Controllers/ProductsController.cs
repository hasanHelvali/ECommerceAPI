using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParameters;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using System.Numerics;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository productWriteRepository;
        readonly private IProductReadRepository productReadRepository;
        readonly private IWebHostEnvironment webHostEnvironment;
        public ProductsController(IProductWriteRepository _productWriteRepository, IProductReadRepository _productReadRepository,
            IWebHostEnvironment _webHostEnvironment)
        {
            this.productWriteRepository = _productWriteRepository;
            this.productReadRepository = _productReadRepository;
            this.webHostEnvironment= _webHostEnvironment;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            //await Task.Delay(500);

            var totalCount = productReadRepository.GetAll().Count();

            var products = productReadRepository.GetAll(false)
                .Skip(pagination.Page * pagination.Size)
                .Take(pagination.Size)
                .Select(p => new
                {
                    p.ID,
                    p.Name,
                    p.Price,
                    p.Stock,
                    p.CreatedDate,
                    p.UpdatedDate
                }).ToList();
            return Ok(new
            {
                products,
                totalCount
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(productReadRepository.GetByIdAsync(id, false));
        }

        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            await productWriteRepository.AddAsync(new Product { Name = model.Name, Price = model.Price, Stock = model.Stock });
            await productWriteRepository.SaveAsync();
            //return Ok();
            return StatusCode((int)HttpStatusCode.Created);
        }


        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await productReadRepository.GetByIdAsync(model.ID);//Guncelleme yapmam gereken product
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.Name = model.Name;
            await productWriteRepository.SaveAsync();
            return Ok(product);
        }

        [HttpDelete("{id}")]//Route uzerinden gelecek olan id parametresini burada belirtmis oldum.
        public async Task<IActionResult> Delete(string id)//route dan ilgili id burada yakalanmıs ve bind edilmis olur.
        {
            await productWriteRepository.RemoveAsync(id);
            await productWriteRepository.SaveAsync();
            return Ok();
        }


        [HttpPost("[action]")]
        /*Post fonksiyonu var oldugu icin bir baska post fonksiyonuna isim vermemiz gerekiyor. 
        //Bu sebeple bu post un kullanılabilmesi icin [action] seklinde bir tanımlama getirdim. Upload adında bir action
        bu post a asla dusmeyiz.*/
        public async Task<IActionResult> Upload()
        {
                string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "resource/products-image");
                /*WebHostEnvironment.webrootpath nesnesi benim wwwroot klasorune erismemiz saglayan bir nesnedir.
                 Burada wwwroot altında rosurce/products-image adında bir kalsor olsuturulacak. Tum bu yol ise 
                Path.Combine ile olusturulup bu izinli yol uploadPath de tutulacak. Biz de aldıgımız resimleri burada, bu yolun belirttigi
                wwwroot icinde resoruce altında products-image kalsorunde tutacaz.
                Buradaki nihai dizin
                wwwrooot/resource/products-image
                seklindeki bir dizine karsılık gelir.*/



                Random rnd = new Random();
                if (Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                foreach (IFormFile item in Request.Form.Files)
                /*Ilgili dosyaları yakalama islemi burada yapılıyor. Gelen bir koleksiyon var ve biz bu koleksiyonun
                 uzerinde donuyoruz.
                */
                {
                    string fullPath = Path.Combine(uploadPath, $"{rnd.Next()}{Path.GetExtension(item.FileName)}");
                    using FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write,
                        FileShare.None, 1024 * 1024, useAsync: false);
                    await item.CopyToAsync(fileStream); 
                    await fileStream.FlushAsync();
                }
            return Ok();
        }

    }
}
 