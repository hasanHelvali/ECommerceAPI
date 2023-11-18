using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParameters;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;
        readonly private IWebHostEnvironment _webHostEnvironment;
        readonly IFileReadRepository _fileReadRepository;
        readonly IFileWriteRepository _fileWriteRepository;
        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IInvioceFileReadRepository _invoiceFileReadRepository;
        readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly IStorageService _storageService;
        readonly IConfiguration _configuration ;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment, IFileReadRepository fileReadRepository, IFileWriteRepository fileWriteRepository,
            IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository,
            IInvioceFileReadRepository invoiceFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository,
            IStorageService storageService, IConfiguration configuration)
        {
            this._productWriteRepository = productWriteRepository;
            this._productReadRepository = productReadRepository;
            this._webHostEnvironment = webHostEnvironment;
            this._fileReadRepository = fileReadRepository;
            this._fileWriteRepository = fileWriteRepository;
            this._productImageFileReadRepository = productImageFileReadRepository;
            this._productImageFileWriteRepository = productImageFileWriteRepository;
            this._invoiceFileReadRepository = invoiceFileReadRepository;
            this._invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            var totalCount = _productReadRepository.GetAll().Count();
            var products = _productReadRepository.GetAll(false)
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
            return Ok(_productReadRepository.GetByIdAsync(id, false));
        }

        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            await _productWriteRepository.AddAsync(new Product { Name = model.Name, Price = model.Price, Stock = model.Stock });
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }


        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.ID);//Guncelleme yapmam gereken product
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.Name = model.Name;
            await _productWriteRepository.SaveAsync();
            return Ok(product);
        }

        [HttpDelete("{id}")]//Route uzerinden gelecek olan id parametresini burada belirtmis oldum.
        public async Task<IActionResult> Delete(string id)//route dan ilgili id burada yakalanmıs ve bind edilmis olur.
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(string id)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photoImages", Request.Form.Files);

            Product product = await _productReadRepository.GetByIdAsync(id);


            await _productImageFileWriteRepository.AddRangeAsync(result.Select(f => new ProductImageFile
            {
                FileName = f.fileName,
                Path = f.pathOrContainerName,
                StorageType = _storageService.StorageName,
                Products = new List<Product>() { product }
            }).ToList());
            await _productImageFileWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages(string id)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImagesFiles)//productImageFiles ile iliskisel olan dosyalar getirilir.
                .FirstOrDefaultAsync(p => p.ID == Guid.Parse(id));//Bunların uzerinden ayıklama yapılır.
            return Ok(product.ProductImagesFiles.Select(p => new
            {
                Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                //p.Path,
                p.FileName,
                p.ID
                //Bana gelen product tan bu yapılari bana don demis oluyorum.
            }));
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage(string id,string imageId)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImagesFiles)
                .FirstOrDefaultAsync(p => p.ID == Guid.Parse(id));
            ProductImageFile? productImageFile = product.ProductImagesFiles.FirstOrDefault(p=>p.ID==Guid.Parse(imageId));
            product.ProductImagesFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
    }
}
