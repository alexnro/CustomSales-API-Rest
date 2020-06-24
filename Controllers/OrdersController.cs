using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRestCustomSales.Models;
using APIRestCustomSales.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRestCustomSales.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase {

        private readonly OrdersService _ordersService;

        public OrdersController(OrdersService ordersService) {
            _ordersService = ordersService;
        }

        [HttpGet]
        public ActionResult<List<Order>> Get() {
            return _ordersService.GetOrders();
        }

        [HttpPost]
        public ActionResult<Order> Add(Order order) {
            _ordersService.AddOrder(order);
            return CreatedAtRoute(new { id = order.Id.ToString() }, order);
        }

        [HttpDelete]
        public IActionResult Delete(string orderId) {
            var order = _ordersService.GetOrderById(orderId);

            if (order == null) {
                return NotFound();
            }

            _ordersService.DeleteOrder(orderId);
            return NoContent();
        }

    }
}
