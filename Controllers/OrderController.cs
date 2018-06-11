using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using MekongSandwichesAPI.Models;
using MekongSandwichesAPI.Repository;

namespace MekongSandwichesAPI.Controllers
{
    [Route("api")]
    public class OrderController : BaseApiController
    {
        private static AppSettings appSettings = new AppSettings();

        OrderRepository orderRepo = null;
        public OrderController(AppSettings _appSettings)
        {
            appSettings = _appSettings;
        }
        [Route("AddOrder")]
        [HttpPost]
        public IActionResult AddOrder([FromBody]Order order)
        {
            //Order order = (Order)JsonConvert.DeserializeObject(JSONorder);
            using (OrderRepository orderRepo = new OrderRepository(appSettings))
            {
                order.id = orderRepo.AddOrder(order);
                return StatusCode(StatusCodes.Status200OK, order.id);
            }
        }

        [Route("getOrders")]
        [HttpGet]
        public IActionResult GetOrders(OrderStatus orderStatus=OrderStatus.New)
        {
             using (OrderRepository orderRepo = new OrderRepository(appSettings))
            {
                return StatusCode(StatusCodes.Status200OK, orderRepo.GetOrders(orderStatus));
            }
        }       
    }
}