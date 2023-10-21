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
        public async Task<IActionResult> Get()
        {
            return Ok("Selamun Aleykum");
        }
    }
}
