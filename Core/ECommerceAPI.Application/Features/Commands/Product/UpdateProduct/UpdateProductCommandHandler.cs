using ECommerceAPI.Application.Repositories;
using P = ECommerceAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ECommerceAPI.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;
        readonly ILogger<UpdateProductCommandHandler> _logger;//Serilog artık kullanılan Log mekanizması oldugundan serilog calısacaktır.

        public UpdateProductCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, ILogger<UpdateProductCommandHandler> logger)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _logger = logger;
        }

        public async  Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest updateProductCommandRequest, CancellationToken cancellationToken)
        {
            P.Product product = await _productReadRepository.GetByIdAsync(updateProductCommandRequest.ID);//Guncelleme yapmam gereken product
            product.Price = updateProductCommandRequest.Price;
            product.Stock = updateProductCommandRequest.Stock;
            product.Name = updateProductCommandRequest.Name;
            await _productWriteRepository.SaveAsync();
            //_logger.LogInformation("Product Guncellendi.");
            return new();
        }
    }
}
