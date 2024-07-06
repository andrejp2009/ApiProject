using Microsoft.AspNetCore.Mvc;
using ApiProject.Data;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using ApiProject.Constants;

namespace ApiProject.Controllers.Orders
{
    [ApiController]
    [Route("api/orders")]
    public class OrderChangeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderChangeController(ApplicationDbContext context)
        {
            _context = context;
        }
        // PUT: api/orders/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing order", Description = "Updates an existing order with the provided details.", Tags = new[] { ApiTags.Orders })]
        public async Task<ActionResult<OrderUpdateResponse>> PutOrder(int id, OrderUpdateRequest orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null)
            {
                return NotFound("Order not found.");
            }

            var user = await _context.Users.FindAsync(orderDto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            existingOrder.ProductName = orderDto.ProductName;
            existingOrder.Price = orderDto.Price;
            existingOrder.OrderDate = orderDto.OrderDate;
            existingOrder.UserId = orderDto.UserId;

            _context.Entry(existingOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound("Order not found.");
                }
                else
                {
                    throw;
                }
            }

            var orderResponseDto = new OrderUpdateResponse
            {
                Id = existingOrder.Id,
                ProductName = existingOrder.ProductName,
                Price = existingOrder.Price,
                OrderDate = existingOrder.OrderDate,
                UserId = existingOrder.UserId
            };

            return Ok(orderResponseDto);
        }
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }        
    }

    // DTO classes
    public class OrderUpdateRequest
    {
        public string ProductName { get; set; }      
        
        public decimal Price { get; set; }           
        public DateTime OrderDate { get; set; }     
        public int UserId { get; set; }            
    }
    public class OrderUpdateResponse
    {
        public int Id { get; set; }                
        public string ProductName { get; set; }      
        
        public decimal Price { get; set; }           
        public DateTime OrderDate { get; set; }     
        public int UserId { get; set; }  
    }       
}