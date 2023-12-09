using ECommerceAPI.Application.Features.Commands.Basket.AddItemToBasket;
using ECommerceAPI.Application.Features.Commands.Basket.RemoveBasketItem;
using ECommerceAPI.Application.Features.Commands.Basket.UpdateQuantity;
using ECommerceAPI.Application.Features.Queries.Basket.GetBasketItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Admin")]
    public class BasketsController : ControllerBase
    {
        readonly IMediator _mediator;

        public BasketsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketItems([FromQuery] GetBasketItemsQueryRequest getBasketItemsQueryRequest)
        {
            List<GetBasketItemsQueryResponse> getBasketItemsQueryResponse = await _mediator.Send(getBasketItemsQueryRequest);
            return Ok(getBasketItemsQueryResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToBasket (AddItemToBasketCommandRequest addItemToBasketCommandRequest)
        {
            AddItemToBasketCommandResponse getBasketItemsQueryResponse = await _mediator.Send(addItemToBasketCommandRequest);
            return Ok(getBasketItemsQueryResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantity(UpdateQuantityCommandRequest updateQuantityCommandRequest)
        {
            UpdateQuantityCommandResponse updateQuantityCommandResponse = await _mediator.Send(updateQuantityCommandRequest);
            return Ok(updateQuantityCommandResponse);
        }

        [HttpDelete("{BasketItemId}")]
        public async Task<IActionResult> RemoveBasketItem([FromRoute] RemoveBasketItemCommandRequest removeBasketItemCommandRequest)
        {
            RemoveBasketItemCommandResponse removeBasketItemCommandResponse  = await _mediator.Send(removeBasketItemCommandRequest);
            return Ok(removeBasketItemCommandResponse);
        }
    }
}
