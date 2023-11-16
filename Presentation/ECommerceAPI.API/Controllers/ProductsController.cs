using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Application.Abstractions.Storage;
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
        readonly  IFileReadRepository fileReadRepository;
        readonly IFileWriteRepository fileWriteRepository;
        readonly IProductImageFileReadRepository productImageFileReadRepository;
        readonly IProductImageFileWriteRepository productImageFileWriteRepository;
        readonly IInvioceFileReadRepository invoiceFileReadRepository ;
        readonly IInvoiceFileWriteRepository invoiceFileWriteRepository  ;
        readonly IStorageService _storageService;

        public ProductsController(IProductWriteRepository _productWriteRepository, IProductReadRepository _productReadRepository,
            IWebHostEnvironment _webHostEnvironment, IFileReadRepository _fileReadRepository, IFileWriteRepository _fileWriteRepository,
            IProductImageFileReadRepository _productImageFileReadRepository, IProductImageFileWriteRepository _productImageFileWriteRepository,
            IInvioceFileReadRepository _invoiceFileReadRepository, IInvoiceFileWriteRepository _invoiceFileWriteRepository, IStorageService storageService)
        {
            this.productWriteRepository = _productWriteRepository;
            this.productReadRepository = _productReadRepository;
            this.webHostEnvironment = _webHostEnvironment;
            this.fileReadRepository = _fileReadRepository;
            this.fileWriteRepository = _fileWriteRepository;
            this.productImageFileReadRepository = _productImageFileReadRepository;
            this.productImageFileWriteRepository = _productImageFileWriteRepository;
            this.invoiceFileReadRepository = _invoiceFileReadRepository;
            this.invoiceFileWriteRepository = _invoiceFileWriteRepository;
            _storageService = storageService;
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
        public async Task<IActionResult> Upload()
        {
            //product images eklenmesi
            //var datas = await fileService.UploadAsync("resource/product-images",Request.Form.Files);
            //var datas = await _storageService.UploadAsync("resource/files", Request.Form.Files);
            var datas = await _storageService.UploadAsync("files", Request.Form.Files);//azure storage icin 

            await productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.pathOrContainerName,
                StorageType=_storageService.StorageName
            }).ToList());
            await productImageFileWriteRepository.SaveAsync();

            //invoice eklenmesi
            //var datas = await fileService.UploadAsync("resource/invoices", Request.Form.Files);
            //await invoiceFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,
            //    Price=new Random().Next()
            //}).ToList());
            //await invoiceFileWriteRepository.SaveAsync();

            //files ekleme
            //var datas = await fileService.UploadAsync("resource/files", Request.Form.Files);
            //await fileWriteRepository.AddRangeAsync(datas.Select(d => new ECommerceAPI.Domain.Entities.File()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,
            //}).ToList());
            //await fileWriteRepository.SaveAsync();


            //var data1 = fileReadRepository.GetAll(false);
            //file icin db deki tum dosyalar getirilir cunku file bir base class tır.

            //var data2 = productImageFileReadRepository.GetAll(false);
            //db den sadece productimage turundeki file lar getirilir.

            //var data3 = invoiceFileReadRepository.GetAll(false);
            //db den sadece invoice turundeki file lar getirilir.



            return Ok();
        }

    }
}
 