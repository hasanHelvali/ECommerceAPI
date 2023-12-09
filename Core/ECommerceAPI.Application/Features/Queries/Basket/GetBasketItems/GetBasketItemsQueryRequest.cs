using MediatR;
using System.Reflection.Metadata.Ecma335;

namespace ECommerceAPI.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryRequest:IRequest<List<GetBasketItemsQueryResponse>>
    {

    }
}