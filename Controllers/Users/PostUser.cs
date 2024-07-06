using Microsoft.AspNetCore.Mvc;
using ApiProject.Data;
using ApiProject.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System;

namespace ApiProject.Controllers.Users
{
    [ApiController]
    [Route("api/users")]
    public class PostUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostUserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/users
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new user", Description = "Creates a new user with the provided details.", Tags = new[] { "USERS"})]
        public async Task<ActionResult<ResponseUserPost>> PostUser(CreateUserPost userDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,                
                Email = userDto.Email,
                DateOfBirth = userDto.DateOfBirth,
                PhoneNumber = userDto.PhoneNumber,
                Address = userDto.Address
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userResponseDto = new ResponseUserPost
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,                
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return CreatedAtAction(nameof(GetUserController.GetUser), "GetUser", new { id = userResponseDto.Id }, userResponseDto);

        }
    }
    
    // DTO classes
    public class CreateUserPost
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class ResponseUserPost
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
}