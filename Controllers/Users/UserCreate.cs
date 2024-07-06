using Microsoft.AspNetCore.Mvc;
using ApiProject.Data;
using ApiProject.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System;
using ApiProject.Constants;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date Format")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "Address cannot be longer than 100 characters")]
        public string Address { get; set; }

        public bool IsActive { get; set; } = true;
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