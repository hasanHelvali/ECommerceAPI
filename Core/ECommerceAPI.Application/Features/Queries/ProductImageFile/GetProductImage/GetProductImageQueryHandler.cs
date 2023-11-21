using ECommerceAPI.Application.Repositories;
using P = ECommerceAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ECommerceAPI.Application.Features.Queries.ProductImageFile.GetProductImage
{
    internal class GetProductImageQueryHandler : IRequestHandler<GetProductImageQueryRequest, List<GetProductImageQueryResponse>>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IConfiguration _configuration;

        public GetProductImageQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            _configuration = configuration;
        }

        public async Task<List<GetProductImageQueryResponse>> Handle(GetProductImageQueryRequest request, CancellationToken cancellationToken)
        {
            P.Product? product = await _productReadRepository.Table.Include(p => p.ProductImagesFiles)
                .FirstOrDefaultAsync(p => p.ID == Guid.Parse(request.ID));
            return product?.ProductImagesFiles.Select(p => new GetProductImageQueryResponse
            {
                ID = p.ID,
                Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                FileName = p.FileName
            }).ToList();
        }
    }
}
