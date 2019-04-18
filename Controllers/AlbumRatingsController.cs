using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mixtape.Models;

namespace Mixtape.Controllers
{
    [Produces("application/json")]
    [Route("api/AlbumRatings")]
    [Authorize]
    [ApiController]
    public class AlbumRatingsController : ControllerBase
    {
        private readonly DataContext _context;

        public AlbumRatingsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/AlbumRatings
        /// <summary>
        /// Gets all of the album ratings
        /// </summary>
        /// <returns></returns>
        /// <response code="200">A list of all album ratings</response>
        [HttpGet]
        public IEnumerable<AlbumRating> GetAlbumRating()
        {
            return _context.AlbumRating;
        }

        // GET: api/AlbumRatings/5
        /// <summary>
        /// Gets a specific album rating
        /// </summary>
        /// <param name="id">The id of the album rating object to get</param>
        /// <returns></returns>
        /// <response code="200">The specific album rating object</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlbumRating([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var albumRating = await _context.AlbumRating.SingleOrDefaultAsync(m => m.AlbumRatingId == id);

            if (albumRating == null)
            {
                return NotFound();
            }

            return Ok(albumRating);
        }

        // PUT: api/AlbumRatings/5
        /// <summary>
        /// Updates a given album rating object
        /// </summary>
        /// <param name="id">The id of the album rating object to update</param>
        /// <param name="albumRating">The updated album rating object</param>
        /// <returns></returns>
        /// <response code="200">No content</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlbumRating([FromRoute] int id, [FromBody] AlbumRating albumRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != albumRating.AlbumRatingId)
            {
                return BadRequest();
            }

            _context.Entry(albumRating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumRatingExists(id))
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

        // POST: api/AlbumRatings
        /// <summary>
        /// Adds a new album rating object to the database
        /// </summary>
        /// <param name="albumRating">A album rating object</param>
        /// <returns></returns>
        /// <response code="200">The added album rating object</response>
        [HttpPost]
        public async Task<IActionResult> PostAlbumRating([FromBody] AlbumRating albumRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AlbumRating.Add(albumRating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlbumRating", new { id = albumRating.AlbumRatingId }, albumRating);
        }

        // DELETE: api/AlbumRatings/5
        /// <summary>
        /// Removes a specific album rating object from the database
        /// </summary>
        /// <param name="id">The id of the album rating object to remove</param>
        /// <returns></returns>
        /// <response code="200">The removed album rating object</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbumRating([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var albumRating = await _context.AlbumRating.SingleOrDefaultAsync(m => m.AlbumRatingId == id);
            if (albumRating == null)
            {
                return NotFound();
            }

            _context.AlbumRating.Remove(albumRating);
            await _context.SaveChangesAsync();

            return Ok(albumRating);
        }

        private bool AlbumRatingExists(int id)
        {
            return _context.AlbumRating.Any(e => e.AlbumRatingId == id);
        }
    }
}
