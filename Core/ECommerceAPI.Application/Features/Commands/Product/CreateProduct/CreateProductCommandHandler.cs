using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        readonly IProductWriteRepository _productWriteRepository;

        public CreateProductCommandHandler(IProductWriteRepository productWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {

            await _productWriteRepository.AddAsync(new ECommerceAPI.Domain.Entities.Product{ Name = request.Name, Price = request.Price, Stock = request.Stock });
            await _productWriteRepository.SaveAsync();
            //return new CreateProductCommandResponse()
            //{

            //};
            return new();
        }
    }
}
