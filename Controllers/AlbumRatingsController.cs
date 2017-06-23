using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mixtape.Models;

namespace mixtape.Controllers
{
    [Produces("application/json")]
    [Route("api/AlbumRatings")]
    public class AlbumRatingsController : Controller
    {
        private readonly mixtapeContext _context;

        public AlbumRatingsController(mixtapeContext context)
        {
            _context = context;
        }

        // GET: api/AlbumRatings
        [HttpGet]
        public IEnumerable<AlbumRating> GetAlbumRating()
        {
            return _context.AlbumRating;
        }

        // GET: api/AlbumRatings/5
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