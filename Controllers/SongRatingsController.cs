using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mixtape.Models;

namespace mixtape.Controllers
{
    [Produces("application/json")]
    [Route("api/SongRatings")]
    public class SongRatingsController : Controller
    {
        private readonly mixtapeContext _context;

        public SongRatingsController(mixtapeContext context)
        {
            _context = context;
        }

        // GET: api/SongRatings
        [HttpGet]
        public IEnumerable<SongRating> GetSongRating()
        {
            return _context.SongRating;
        }

        // GET: api/SongRatings/5
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