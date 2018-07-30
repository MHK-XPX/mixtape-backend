using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Mixtape.Models;

namespace Mixtape.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly DataContext _context;

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

            /*
             * For some reason, when we set the object to modified, if we give it a null field, it will set it to null (even if blank as well)
             * There is probably a better way of doing this, but for now this will work
             */
            if (user.FirstName == null || user.FirstName.Length <= 0)
            {

                user.FirstName = _context.User.AsNoTracking().SingleOrDefault(m => m.UserId == id).FirstName;
            }
            if (user.LastName == null || user.LastName.Length <= 0)
            {

                user.LastName = _context.User.AsNoTracking().SingleOrDefault(m => m.UserId == id).LastName;
            }
            if (user.Username == null || user.Username.Length <= 0)
            {

                user.Username = _context.User.AsNoTracking().SingleOrDefault(m => m.UserId == id).Username;
            }
            if (user.Password == null || user.Password.Length <= 0)
            {

                user.Password = _context.User.AsNoTracking().SingleOrDefault(m => m.UserId == id).Password;
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
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User _user = _context.User.SingleOrDefault(x => x.Username == user.Username);

            //If the username is already taken we return a 400 error
            if (_user != null)
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
        /// <summary>
        /// Checks if a given username is taken or not
        /// </summary>
        /// <param name="username">The username to check</param>
        /// <returns></returns>
        /// <response code="200">Username is not taken</response>
        /// <response code="400">Username is taken</response>
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
