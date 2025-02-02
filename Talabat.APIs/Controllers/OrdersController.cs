using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService
            ,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto order)
        {
            var address = _mapper.Map<AddressDto,Address>(order.ShippingAddress);
            var createdOrder =  await _orderService.CreateOrderAsync(order.BuyerEmail, order.BasketId, order.DeliveryMethodId, address);
            if (createdOrder == null) return BadRequest(new ApiResponse(400));
            return Ok(createdOrder);
        }
    }
}
