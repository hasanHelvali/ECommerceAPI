using ECommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.ProductImageFile.ChangeShowCaseImage
{
    public class ChangeShowCaseImageCommandHandler : IRequestHandler<ChangeShowCaseImageCommandRequest, ChangeShowCaseImageCommandResponse>
    {
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ChangeShowCaseImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowCaseImageCommandResponse> Handle(ChangeShowCaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            var query = _productImageFileWriteRepository.Table
                 .Include(p => p.Products)
                 .SelectMany(p => p.Products, (pif, p) => new
                 {
                     pif,
                     p
                 });
            var data = await query
                 .FirstOrDefaultAsync(p => p.p.ID == Guid.Parse(request.ProductId) && p.pif.Showcase);
            if (data != null)
                data.pif.Showcase = false;



            var image = await query.FirstOrDefaultAsync(P => P.pif.ID == Guid.Parse(request.ImageId));
            if (image != null)
                image.pif.Showcase = true;

            await _productImageFileWriteRepository.SaveAsync();
            return new();

        }
    }
}
//1.08