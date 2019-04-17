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
    [Route("api/Albums")]
    [Authorize]
    public class AlbumsController : Controller
    {
        private readonly DataContext _context;

        public AlbumsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Albums
        /// <summary>
        /// Gets all albums
        /// </summary>
        /// <returns></returns>
        /// <response code="200">A list of all albums</response>
        [HttpGet]
        public IEnumerable<Album> GetAlbum()
        {
            return _context.Album;
        }

        // GET: api/Albums/5
        /// <summary>
        /// Gets a specific album
        /// </summary>
        /// <param name="id">The id of the specific album</param>
        /// <returns></returns>
        /// <response code="200">An album object</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlbum([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var album = await _context.Album
                                .Include(albumSong => albumSong.Song)
                                .SingleOrDefaultAsync(m => m.AlbumId == id);

            if (album == null)
            {
                return NotFound();
            }

            return Ok(album);
        }

        // PUT: api/Albums/5
        /// <summary>
        /// Updates a specific album object
        /// </summary>
        /// <param name="id">The id of the album to update</param>
        /// <param name="album">The updated album object</param>
        /// <returns></returns>
        /// <response code="200">No content</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlbum([FromRoute] int id, [FromBody] Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != album.AlbumId)
            {
                return BadRequest();
            }

            _context.Entry(album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
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

        // POST: api/Albums
        /// <summary>
        /// Adds an album object to the database
        /// </summary>
        /// <param name="album">An album object</param>
        /// <returns></returns>
        /// <response code="200">The added album object</response>
        [HttpPost]
        public async Task<IActionResult> PostAlbum([FromBody] Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Album.Add(album);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlbum", new { id = album.AlbumId }, album);
        }

        // DELETE: api/Albums/5
        /// <summary>
        /// Removes a specific album object from the database
        /// </summary>
        /// <param name="id">The id of the album object to remove</param>
        /// <returns></returns>
        /// <response code="200">The removed album object</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var album = await _context.Album.SingleOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }

            _context.Album.Remove(album);
            await _context.SaveChangesAsync();

            return Ok(album);
        }

        private bool AlbumExists(int id)
        {
            return _context.Album.Any(e => e.AlbumId == id);
        }
    }
}
