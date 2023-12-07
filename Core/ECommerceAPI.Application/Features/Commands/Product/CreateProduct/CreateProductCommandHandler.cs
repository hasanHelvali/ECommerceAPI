using ECommerceAPI.Application.Abstractions.Hubs;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        readonly IProductHubService _productHubService;
        public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductHubService productHubService)
        {
            _productWriteRepository = productWriteRepository;
            _productHubService = productHubService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {

            await _productWriteRepository.AddAsync(new ECommerceAPI.Domain.Entities.Product{ Name = request.Name, Price = request.Price, Stock = request.Stock });
            await _productWriteRepository.SaveAsync();
            await _productHubService.ProductAddedMessageAsync($"{request.Name} Isminde Ürün Eklenmiştir.");
            return new();
        }
    }
}
