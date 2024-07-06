using Microsoft.AspNetCore.Mvc;
using ApiProject.Data;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using ApiProject.Constants;


namespace ApiProject.Controllers.Orders
{
    [ApiController]
    [Route("api/orders")]
    public class OrderDeleteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderDeleteController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an order by ID", Description = "Deletes an order by the given ID.", Tags = new[] { ApiTags.Orders })]
        [SwaggerResponse(204, "No Content - User successfully deleted.")]
        [SwaggerResponse(404, "Not Found - User not found.")]
        [SwaggerResponse(500, "Internal Server Error - An error occurred while deleting the user.")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }   
}