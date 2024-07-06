using Microsoft.AspNetCore.Mvc;
using ApiProject.Data;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using ApiProject.Constants;

namespace ApiProject.Controllers.Users
{
    [ApiController]
    [Route("api/users")]
    public class UserDeleteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserDeleteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a user by ID", Description = "Deletes a user by their ID.", Tags = new[] { ApiTags.Users })]
        [SwaggerResponse(204, "No Content - User successfully deleted.")]
        [SwaggerResponse(404, "Not Found - User not found.")]
        [SwaggerResponse(500, "Internal Server Error - An error occurred while deleting the user.")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
