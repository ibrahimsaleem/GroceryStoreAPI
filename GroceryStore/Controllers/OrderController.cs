using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Dto;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStore.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get the list of all the orders placed by a particular user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<List<OrdersDto>> Get(int userId)
        {
            return await Task.FromResult(_orderService.GetOrderList(userId)).ConfigureAwait(true) ;
        }


        /// <summary>
        /// Get the list of all orders
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<ActionResult<List<OrdersDto>>> GetAllOrders()
        {
            var allOrders = await Task.FromResult(_orderService.GetAllOrders()).ConfigureAwait(true);
            return Ok(allOrders);
        }

    }
}
