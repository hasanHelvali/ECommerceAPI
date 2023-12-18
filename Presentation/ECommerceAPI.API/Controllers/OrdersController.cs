using ECommerceAPI.Application.Features.Commands.Order.CompleteOrder;
using ECommerceAPI.Application.Features.Commands.Order.CreateOrder;
using ECommerceAPI.Application.Features.Queries.Order.GetAllOrders;
using ECommerceAPI.Application.Features.Queries.Order.GetOrderById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class OrdersController : ControllerBase
    {
        readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest getAllOrdersQueryRequest)
        {
            GetAllOrdersQueryResponse getAllOrdersQueryResponse = await _mediator.Send(getAllOrdersQueryRequest);
            return Ok(getAllOrdersQueryResponse);
        }
      
        [HttpGet("Id")]
        public async Task<IActionResult> GetOrderById([FromRoute] GetOrderByIdQueryRequest getOrderByIdQueryRequest)
        {
            GetOrderByIdQueryResponse getOrderByIdQueryResponse = await _mediator.Send(getOrderByIdQueryRequest);
            return Ok(getOrderByIdQueryResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest createOrderCommandRequest)
        {
            CreateOrderCommandResponse createOrderCommandResponse = await _mediator.Send(createOrderCommandRequest);
            return Ok(createOrderCommandResponse);
        }


        [HttpGet("complete-order/{id}")]
        public async Task<IActionResult> CompleteOrder([FromRoute] CompleteOrderCommandRequest completeOrderCommandRequest)
        {
            CompleteOrderCommandResponse completeOrderCommandResponse = await _mediator.Send(completeOrderCommandRequest);
            return Ok(completeOrderCommandResponse);
        }

    }
}
