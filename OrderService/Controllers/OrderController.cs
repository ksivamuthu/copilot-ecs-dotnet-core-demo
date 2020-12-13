using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly OrderService _orderService;

    public OrderController(ILogger<OrderController> logger, OrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<Order> CreateOrder([FromBody] Order order)
    {
        order.OrderId = Guid.NewGuid().ToString();
        return await _orderService.Create(order);
    }

    [HttpGet]
    [Route("{orderId}")]
    public async Task<Order> GetOrder(string orderId) 
    {
        return await _orderService.GetById(orderId);
    }
}
