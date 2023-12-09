using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Basket.UpdateQuantity
{
    public class UpdateQuantityCommandHandler : IRequestHandler<UpdateQuantityCommandRequest, UpdateQuantityCommandResponse>
    {
        readonly IBasketService _basketService;

        public UpdateQuantityCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<UpdateQuantityCommandResponse> Handle(UpdateQuantityCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.UpdateQuantityAsync(new ViewModels.Baskets.VM_UpdateBasketItems()
            {
                BasketItemId=request.BasketItemId,
                Quantity=request.Quantity
            });
            return new();
        }
    }
}
