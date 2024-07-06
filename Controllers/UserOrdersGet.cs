using Microsoft.AspNetCore.Mvc;
using ApiProject.Data;
using ApiProject.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiProject.Constants;
using System;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/user/{userId}/orders")]
    public class UserOrdersGetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserOrdersGetController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("details")]
        [SwaggerOperation(Summary = "Get user with orders", Description = "Retrieves the user along with their associated orders based on the provided user ID.", Tags = new[] { ApiTags.UserData })]
        public async Task<ActionResult<UserOrdersGetResponse>> GetUserWithOrders(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.Where(o => o.UserId == userId).ToListAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                IsActive = user.IsActive
            };

            var orderDtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                ProductName = o.ProductName,
                Price = o.Price,
                OrderDate = o.OrderDate,
                UserId = o.UserId
            }).ToList();

            var userOrdersDto = new UserOrdersGetResponse
            {
                User = userDto,
                Orders = orderDtos
            };

            return userOrdersDto;
        }

        // Добавляем новый метод контроллера для получения заказов по ID пользователя
        [HttpGet]
        [SwaggerOperation(Summary = "Get orders by User ID", Description = "Returns a list of orders for the given User ID.", Tags = new[] { ApiTags.UserData })]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserId(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(order => new OrderDto
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

    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
    }

    public class UserOrdersGetResponse
    {
        public UserDto User { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
