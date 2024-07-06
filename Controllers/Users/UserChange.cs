using Microsoft.AspNetCore.Mvc;
using ApiProject.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;
using System;
using ApiProject.Constants;


namespace ApiProject.Controllers.Users
{
    [ApiController]
    [Route("api/users")]
    public class UserChangeCOntroller : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserChangeCOntroller(ApplicationDbContext context)
        {
            _context = context;
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing user", Description = "Updates an existing user with the provided details.", Tags = new[] { ApiTags.Users })]
        public async Task<ActionResult<UserChangeResponse>> PutUser(int id, UserChangeRequest userDto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Update fields that can be changed
            user.Email = userDto.Email;
            user.DateOfBirth = userDto.DateOfBirth;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Address = userDto.Address;
            user.IsActive = userDto.IsActive;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var userResponseDto = new UserChangeResponse
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

                return Ok(userResponseDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
    }

    // DTO classes
    public class UserChangeRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserChangeResponse
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