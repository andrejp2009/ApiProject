using Microsoft.AspNetCore.Mvc;
using ApiProject.Data;
using ApiProject.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using ApiProject.Constants;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/userorders")]
    public class GetUserOrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GetUserOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "Get user with orders", Description = "Retrieves the user along with their associated orders based on the provided user ID.", Tags = new[] { ApiTags.UserData })]
        public async Task<ActionResult<GetUserOrders>> GetUserWithOrders(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.Where(o => o.UserId == userId).ToListAsync();

            var userOrdersDto = new GetUserOrders
            {
                User = user,
                Orders = orders
            };

            return userOrdersDto;
        }
    }
    
    public class GetUserOrders
    {
        public User User { get; set; }
        public List<Order> Orders { get; set; }
    }

}