using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParameters;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        readonly ILogger<GetAllProductQueryHandler> _logger;
        public GetAllProductQueryHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, ILogger<GetAllProductQueryHandler> logger)
        {
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get All Products");
            var totalCount = _productReadRepository.GetAll().Count();
            var products = _productReadRepository.GetAll(false)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(p => new
                {
                    p.ID,
                    p.Name,
                    p.Price,
                    p.Stock,
                    p.CreatedDate,
                    p.UpdatedDate
                }).ToList();

            return new()
            {
                Products = products,
                TotalCount = totalCount
            };
        }
    }
}
