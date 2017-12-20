using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Mixtape.Models;

namespace Mixtape.Controllers
{
    [Produces("application/json")]
    [Route("api/Artists")]
    public class ArtistsController : Controller
    {
        private readonly DataContext _context;

        public ArtistsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Artists
        [HttpGet]
        public IEnumerable<Artist> GetArtist()
        {
            return _context.Artist;
        }

        // GET: api/Artists/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtist([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artist = await _context.Artist.SingleOrDefaultAsync(m => m.ArtistId == id);

            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artist);
        }

        //GET: api/Artists/Spec/5
        /// <summary>
        /// Returns an Artist with all of their albums AND songs per album
        /// </summary>
        /// <param name="id">The ID of the artist</param>
        /// <returns></returns>
        [HttpGet("Spec/{id}")]
        public async Task<IActionResult> GetSpecificArtist([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artist = await _context.Artist
                                 .Include(alb => alb.Album)
                                    .ThenInclude(s => s.Song)
                                .SingleOrDefaultAsync(m => m.ArtistId == id);

            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artist);
        }

        // PUT: api/Artists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtist([FromRoute] int id, [FromBody] Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != artist.ArtistId)
            {
                return BadRequest();
            }

            _context.Entry(artist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
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

        // POST: api/Artists
        [HttpPost]
        public async Task<IActionResult> PostArtist([FromBody] Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Artist.Add(artist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtist", new { id = artist.ArtistId }, artist);
        }

        // DELETE: api/Artists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artist = await _context.Artist.SingleOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            _context.Artist.Remove(artist);
            await _context.SaveChangesAsync();

            return Ok(artist);
        }

        private bool ArtistExists(int id)
        {
            return _context.Artist.Any(e => e.ArtistId == id);
        }
    }
}
