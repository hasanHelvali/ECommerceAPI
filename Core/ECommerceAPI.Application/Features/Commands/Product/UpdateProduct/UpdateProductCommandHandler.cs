using ECommerceAPI.Application.Repositories;
using P = ECommerceAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;

        public UpdateProductCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async  Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest updateProductCommandRequest, CancellationToken cancellationToken)
        {
            P.Product product = await _productReadRepository.GetByIdAsync(updateProductCommandRequest.ID);//Guncelleme yapmam gereken product
            product.Price = updateProductCommandRequest.Price;
            product.Stock = updateProductCommandRequest.Stock;
            product.Name = updateProductCommandRequest.Name;
            await _productWriteRepository.SaveAsync();
            return new();
        }
    }
}
