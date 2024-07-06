using Microsoft.AspNetCore.Mvc;
using ApiProject.Data;
using ApiProject.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System;
using ApiProject.Controllers.Users;
using ApiProject.Constants;

namespace ApiProject.Controllers.Orders
{
    [ApiController]
    [Route("api/orders")]
    public class PostOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostOrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        // POST: api/orders
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new order", Description = "Creates a new order with the provided details.", Tags = new[] { ApiTags.Orders })]
        public async Task<ActionResult<ResponseOrderPost>> PostOrder(CreateOrderPost orderDto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(orderDto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var order = new Order
            {
                ProductName = orderDto.ProductName,
                Price = orderDto.Price,
                OrderDate = orderDto.OrderDate,
                UserId = orderDto.UserId,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderResponseDto = new ResponseOrderPost
            {
                Id = order.Id,
                ProductName = order.ProductName,
                Price = order.Price,
                OrderDate = order.OrderDate,
                UserId = order.UserId
            };

            return CreatedAtAction(nameof(GetUserController.GetUser), new { id = orderResponseDto.Id }, orderResponseDto);
        }        
    }
    
    // DTO classes
    public class CreateOrderPost
    {             
        public string ProductName { get; set; }      
        
        public decimal Price { get; set; }           
        public DateTime OrderDate { get; set; }     
        public int UserId { get; set; }             

    }

    public class ResponseOrderPost
    {
        public int Id { get; set; }                
        public string ProductName { get; set; }      
        
        public decimal Price { get; set; }           
        public DateTime OrderDate { get; set; }     
        public int UserId { get; set; }             
    }      
}