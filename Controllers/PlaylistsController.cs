using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mixtape.Models;

namespace mixtape.Controllers
{
    [Produces("application/json")]
    [Route("api/Playlists")]
    public class PlaylistsController : Controller
    {
        private readonly mixtapeContext _context;

        public PlaylistsController(mixtapeContext context)
        {
            _context = context;
        }

        // GET: api/Playlists
        [HttpGet]
        public IEnumerable<Playlist> GetPlaylist()
        {
            return _context.Playlist
                .Include(m => m.PlaylistSong)
                .ToList();
        }

        // GET: api/Playlists/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaylist([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playlist = await _context.Playlist.SingleOrDefaultAsync(m => m.PlaylistId == id);
            await _context.Entry(playlist).Collection(m => m.PlaylistSong).LoadAsync();

            if (playlist == null)
            {
                return NotFound();
            }

            return Ok(playlist);
        }

        // PUT: api/Playlists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaylist([FromRoute] int id, [FromBody] Playlist playlist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playlist.PlaylistId)
            {
                return BadRequest();
            }

            _context.Entry(playlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaylistExists(id))
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

        // POST: api/Playlists
        [HttpPost]
        public async Task<IActionResult> PostPlaylist([FromBody] Playlist playlist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Playlist.Add(playlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlaylist", new { id = playlist.PlaylistId }, playlist);
        }

        // DELETE: api/Playlists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playlist = await _context.Playlist.SingleOrDefaultAsync(m => m.PlaylistId == id);
            if (playlist == null)
            {
                return NotFound();
            }

            _context.Playlist.Remove(playlist);
            await _context.SaveChangesAsync();

            return Ok(playlist);
        }

        private bool PlaylistExists(int id)
        {
            return _context.Playlist.Any(e => e.PlaylistId == id);
        }
    }
}