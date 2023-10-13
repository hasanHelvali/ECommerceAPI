using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository productWriteRepository;
        private readonly IProductReadRepository productReadRepository;

        public ProductsController(IProductWriteRepository productWriteRepository,IProductReadRepository productReadRepository)
        {
            this.productWriteRepository = productWriteRepository;
            this.productWriteRepository = productWriteRepository;
        }


        [HttpGet]
        public async Task Get()
        {
            //await productWriteRepository.AddRangeAsync(new()
            //{
            //    new(){ID=Guid.NewGuid(),Name="Product1",Price=100,CreatedDate=DateTime.UtcNow,Stock=10},
            //    new(){ID=Guid.NewGuid(),Name="Product2",Price=200,CreatedDate=DateTime.UtcNow,Stock=20},
            //    new(){ID=Guid.NewGuid(),Name="Product3",Price=300,CreatedDate=DateTime.UtcNow,Stock=30},
            //});

            //var count = await productWriteRepository.SaveAsync();
            Product p = await productReadRepository.GetByIdAsync("AAF0BFA8-3C0B-4C9B-B810-E84EB015EE54");
            p.Name = "Ahmet";
            await productWriteRepository.SaveAsync();
            /*Burada productReadRepository uzerinden degisen bir datayi productWriteRepository uzerinden nasıl kaydediyoruz.
            Isın sırrı scoped da dir. Bu yuzden ilgili service leri IoC ye Scoped olarak ekledik.
            Sımdilik hem write hem de read operasyonlarında kullanacagımız dbcontext aynı olacagından dolayı bunu yapabiliyoruz.*/
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Product product= await productReadRepository.GetByIdAsync(id);
            return Ok(product);
        }
    }
}
