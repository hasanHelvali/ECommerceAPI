using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Services
{
    public class ProductService : IProductService
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;
        readonly IQRCodeService _qRCodeService;

        public ProductService(IProductReadRepository productReadRepository, IQRCodeService qRCodeService, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _qRCodeService = qRCodeService;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<byte[]> QRCodeToProductAsync(string productId)
        {
            Product product =await _productReadRepository.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product Not Found");

            var plainObject = new
            {
                product.ID,
                product.Name,
                product.Price,
                product.Stock,
                product.CreatedDate,
            };
            string plainText=JsonSerializer.Serialize(plainObject);
            return _qRCodeService.GenerateQRCode(plainText);

        }

        public async Task StockUpdateToProductAsync(string productId, int stock)
        {
            Product product = await _productReadRepository.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product Not Found");
            product.Stock = stock;
            await _productWriteRepository.SaveAsync();
        }
    }
}
