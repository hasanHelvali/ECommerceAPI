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
        readonly IQRCodeService _qRCodeService;

        public ProductService(IProductReadRepository productReadRepository, IQRCodeService qRCodeService)
        {
            _productReadRepository = productReadRepository;
            _qRCodeService = qRCodeService;
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
    }
}
