using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Mixtape.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.ObjectModel;

namespace Mixtape.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly DataContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>All user entities</returns>
        /// <response code="200">User entities</response>
        /// <response code="400">Error model</response>
        [HttpGet]
        //[ProducesResponseType(typeof(ICollection<User>), 200)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(200, Type = typeof(User), Description = "User objects returned successfully")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
        public IEnumerable<User> GetUser()
        {
            return _context.User;
        }

        // GET: api/Users/5
        /// <summary>
        /// Get a given user
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <returns>A given user entity</returns>
        /// <response code="200">User entity</response>
        /// <response code="400">Error model</response>
        [HttpGet("{id}")]
        //[ProducesResponseType(typeof(User), 200)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(200, Type = typeof(User), Description = "User object returned successfully")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        /// <summary>
        /// Updates a given user
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <param name="user">The updated user entity</param>
        /// <returns>An empty object</returns>
        /// <response code="200">User sucessfully updated</response>
        /// <response code="400">Error model</response>
        [HttpPut("{id}")]
        //[ProducesResponseType(typeof(User), 200)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(200, Type = typeof(User), Description = "User updated successfully")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">The new user to post</param>
        /// <returns>An new user object</returns>
        /// <response code="201">User sucessfully created</response>
        /// <response code="400">Error model</response>
        [HttpPost]
        //[ProducesResponseType(typeof(User), 201)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(201, Type = typeof(User), Description = "User successfully created")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User _user = _context.User.SingleOrDefault(x => x.Username == user.Username);

            //If the username is already taken we return a 400 error
            if(_user != null)
            {
                ModelState.AddModelError("Error", "Username already taken");
                return BadRequest(ModelState);
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        /// <summary>
        /// Delete a given user
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <returns>An empty object</returns>
        /// <response code="200">User sucessfully deleted</response>
        /// <response code="400">Error model</response>
        [HttpDelete("{id}")]
        //[ProducesResponseType(typeof(User), 200)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(200, Type = typeof(User), Description = "User deleted successfully")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // api/Users/Check/<username>
        [HttpGet("Check/{username}")]
        public async Task<IActionResult> CheckForUsername([FromRoute] string username)
        {
            User _user = _context.User.SingleOrDefault(x => x.Username == username);

            //If the username is already taken we return a 400 error
            if (_user != null)
            {
                ModelState.AddModelError("Error", "Username already taken");
                return BadRequest(ModelState);
            }

            return Ok();
        }
        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
