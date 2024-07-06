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
    public class PutOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PutOrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        // PUT: api/orders/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing order", Description = "Updates an existing order with the provided details.", Tags = new[] { ApiTags.Orders })]
        public async Task<ActionResult<ResponseOrderPut>> PutOrder(int id, UpdateOrderPut orderDto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderDto.UserId)
            {
                return BadRequest("Order ID does not match User ID.");
            }

            // Проверяем существование заказа с указанным ID
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

            return NoContent();
        }
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }        
    }

    // DTO classes
    public class UpdateOrderPut
    {
        public string ProductName { get; set; }      
        
        public decimal Price { get; set; }           
        public DateTime OrderDate { get; set; }     
        public int UserId { get; set; }            
    }
    public class ResponseOrderPut
    {
        public int Id { get; set; }                
        public string ProductName { get; set; }      
        
        public decimal Price { get; set; }           
        public DateTime OrderDate { get; set; }     
        public int UserId { get; set; }  
    }       
}