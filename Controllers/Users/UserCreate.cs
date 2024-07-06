using Microsoft.AspNetCore.Mvc;
using ApiProject.Data;
using ApiProject.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System;
using ApiProject.Constants;

namespace ApiProject.Controllers.Users
{
    [ApiController]
    [Route("api/users")]
    public class UserCreateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserCreateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/users
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new user", Description = "Creates a new user with the provided details.", Tags = new[] { ApiTags.Users })]
        public async Task<ActionResult<UserCreateResponse>> PostUser(UserCreateRequest userDto)
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

            var userResponseDto = new UserCreateResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return Ok(userResponseDto);
        } 
    }
    
    // DTO classes
    public class UserCreateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class UserCreateResponse
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