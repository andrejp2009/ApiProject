using Microsoft.AspNetCore.Mvc;
using ApiProject.Data;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiProject.Controllers.Orders
{
    [ApiController]
    [Route("api/orders")]
    public class GetOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GetOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        [SwaggerOperation(Summary = "Get all orders", Description = "Returns a list of all orders.", Tags = new[] { "ORDERS" })]
        public async Task<ActionResult<IEnumerable<GetOrder>>> GetOrders()
        {
            var orders = await _context.Orders
                .Select(order => new GetOrder
                {
                    Id = order.Id,
                    ProductName = order.ProductName,
                    Price = order.Price,
                    OrderDate = order.OrderDate,
                    UserId = order.UserId
                })
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get an order by ID", Description = "Returns an order by the given ID.", Tags = new[] { "ORDERS"})]
        public async Task<ActionResult<GetOrder>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Select(order => new GetOrder
                {
                    Id = order.Id,
                    ProductName = order.ProductName,
                    Price = order.Price,
                    OrderDate = order.OrderDate,
                    UserId = order.UserId
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // GET: api/orders/user/{userId}
        [HttpGet("user/{userId}")]
        [SwaggerOperation(Summary = "Get orders by User ID", Description = "Returns a list of orders for the given User ID.", Tags = new[] { "USERS"})]
        public async Task<ActionResult<IEnumerable<GetOrder>>> GetOrdersByUserId(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(order => new GetOrder
                {
                    Id = order.Id,
                    ProductName = order.ProductName,
                    Price = order.Price,
                    OrderDate = order.OrderDate,
                    UserId = order.UserId
                })
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return Ok(orders);
        }
    }
    
    //Dto classes
    public class GetOrder
    {
        public int Id { get; set; }                
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }  
        public int UserId { get; set; }           

    }
}