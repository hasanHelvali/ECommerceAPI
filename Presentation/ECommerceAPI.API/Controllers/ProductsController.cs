using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParameters;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository productWriteRepository;
        readonly private IProductReadRepository productReadRepository;
        public ProductsController(IProductWriteRepository _productWriteRepository, IProductReadRepository _productReadRepository)
        {
            this.productWriteRepository = _productWriteRepository;
            this.productReadRepository = _productReadRepository;
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


        //4 tane temel fonksiyonumuzu test edebilecegimiz controller yapımız mevcut.
    }
}
