using Microsoft.AspNetCore.Mvc;
using ApiProject.Data;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using ApiProject.Constants;

namespace ApiProject.Controllers.Users
{
    [ApiController]
    [Route("api/users")]
    public class GetUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GetUserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/users/{id}
        [HttpGet("{id}", Name = "GetUser")]
        [SwaggerOperation(Summary = "Get a user by ID", Description = "Retrieves a user by their ID.", Tags = new[] { ApiTags.Users })]
        public async Task<ActionResult<GetUser>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new GetUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,                
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return userDto;
        }

        // GET: api/users/search
        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search users by name or active status", Description = "Searches for users by their First name or whether they are active.", Tags = new[] { ApiTags.Users })]
        public async Task<ActionResult<IEnumerable<GetUser>>> SearchUsers([FromQuery] string name, [FromQuery] bool? isActive)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => u.FirstName.Contains(name));
            }

            if (isActive.HasValue)
            {
                query = query.Where(u => u.IsActive == isActive.Value);
            }

            var users = await query.ToListAsync();

            var userDtos = users.Select(user => new GetUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,                
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address

            }).ToList();

            return Ok(userDtos);
        }
    }
    
    // DTO classes
    public class GetUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }    
}