using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Mixtape.Models;

namespace Mixtape.Controllers
{
    /// <summary>
    /// Not implemented yet
    /// </summary>
    [Produces("application/json")]
    [Route("api/SongRatings")]
    public class SongRatingsController : Controller
    {
        private readonly DataContext _context;

        public SongRatingsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/SongRatings
        /// <summary>
        /// A list of all song ratings 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">A list of song rating</response>
        [HttpGet]
        public IEnumerable<SongRating> GetSongRating()
        {
            return _context.SongRating;
        }

        // GET: api/SongRatings/5
        /// <summary>
        /// Gets a specific song rating
        /// </summary>
        /// <param name="id">The id of the song rating to get</param>
        /// <returns></returns>
        /// <response code="200">A song rating object</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSongRating([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var songRating = await _context.SongRating.SingleOrDefaultAsync(m => m.SongRatingId == id);

            if (songRating == null)
            {
                return NotFound();
            }

            return Ok(songRating);
        }

        // PUT: api/SongRatings/5
        /// <summary>
        /// Updates a given song rating
        /// </summary>
        /// <param name="id">The id of the song rating to update</param>
        /// <param name="songRating">The updated song rating object</param>
        /// <returns></returns>
        /// <response code="200">No content</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSongRating([FromRoute] int id, [FromBody] SongRating songRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != songRating.SongRatingId)
            {
                return BadRequest();
            }

            _context.Entry(songRating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongRatingExists(id))
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

        // POST: api/SongRatings
        /// <summary>
        /// Adds a song rating object to the database
        /// </summary>
        /// <param name="songRating">The song rating object to add</param>
        /// <returns></returns>
        /// <response code="200">The added song rating object</response>
        [HttpPost]
        public async Task<IActionResult> PostSongRating([FromBody] SongRating songRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SongRating.Add(songRating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSongRating", new { id = songRating.SongRatingId }, songRating);
        }

        // DELETE: api/SongRatings/5
        /// <summary>
        /// Removes a song rating object from the database
        /// </summary>
        /// <param name="id">The id of the song rating object to remove from the database</param>
        /// <returns></returns>
        /// <response code="200">The removed song rating object</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSongRating([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var songRating = await _context.SongRating.SingleOrDefaultAsync(m => m.SongRatingId == id);
            if (songRating == null)
            {
                return NotFound();
            }

            _context.SongRating.Remove(songRating);
            await _context.SaveChangesAsync();

            return Ok(songRating);
        }

        private bool SongRatingExists(int id)
        {
            return _context.SongRating.Any(e => e.SongRatingId == id);
        }
    }
}
