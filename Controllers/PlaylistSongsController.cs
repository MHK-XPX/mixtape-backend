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
    [Route("api/PlaylistSongs")]
    public class PlaylistSongsController : Controller
    {
        private readonly mixtapeContext _context;

        public PlaylistSongsController(mixtapeContext context)
        {
            _context = context;
        }

        // GET: api/PlaylistSongs
        [HttpGet]
        public IEnumerable<PlaylistSong> GetPlaylistSong()
        {
            return _context.PlaylistSong;
        }

        // GET: api/PlaylistSongs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaylistSong([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playlistSong = await _context.PlaylistSong.SingleOrDefaultAsync(m => m.PlaylistSongId == id);

            if (playlistSong == null)
            {
                return NotFound();
            }

            return Ok(playlistSong);
        }

        // PUT: api/PlaylistSongs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaylistSong([FromRoute] int id, [FromBody] PlaylistSong playlistSong)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playlistSong.PlaylistSongId)
            {
                return BadRequest();
            }

            _context.Entry(playlistSong).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaylistSongExists(id))
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

        // POST: api/PlaylistSongs
        [HttpPost]
        public async Task<IActionResult> PostPlaylistSong([FromBody] PlaylistSong playlistSong)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PlaylistSong.Add(playlistSong);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlaylistSong", new { id = playlistSong.PlaylistSongId }, playlistSong);
        }

        // DELETE: api/PlaylistSongs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylistSong([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playlistSong = await _context.PlaylistSong.SingleOrDefaultAsync(m => m.PlaylistSongId == id);
            if (playlistSong == null)
            {
                return NotFound();
            }

            _context.PlaylistSong.Remove(playlistSong);
            await _context.SaveChangesAsync();

            return Ok(playlistSong);
        }

        private bool PlaylistSongExists(int id)
        {
            return _context.PlaylistSong.Any(e => e.PlaylistSongId == id);
        }
    }
}