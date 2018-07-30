using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Mixtape.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Azure.KeyVault.Models;

namespace Mixtape.Controllers
{
    [Produces("application/json")]
    [Route("api/PlaylistSongs")]
    public class PlaylistSongsController : Controller
    {
        private readonly DataContext _context;

        public PlaylistSongsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/PlaylistSongs
        /// <summary>
        /// Get all playlistsongs
        /// </summary>
        /// <returns>All playlistsong entities</returns>
        /// <response code="200">Playlistsong entities</response>
        /// <response code="400">Error model</response>
        [HttpGet]
        public IEnumerable<PlaylistSong> GetPlaylistSong()
        {
            return _context.PlaylistSong;
        }

        // GET: api/PlaylistSongs/5
        /// <summary>
        /// Get a given playlistsong
        /// </summary>
        /// <param name="id">ID of the playlistsong</param>
        /// <returns>A given playlistsong entity</returns>
        /// <response code="200">Playlistsong entity</response>
        /// <response code="400">Error model</response>
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
        /// <summary>
        /// Updates a given playlistsong
        /// </summary>
        /// <param name="id">ID of the playlistsong</param>
        /// <param name="playlistSong">The updated playlistsong entity</param>
        /// <returns>A new playlistsong object</returns>
        /// <response code="200">Playlistsong sucessfully updated</response>
        /// <response code="400">Error model</response>
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
        /// <summary>
        /// Creates a new playlistsong
        /// </summary>
        /// <param name="playlistSong">The new playlistsong to post</param>
        /// <returns>An empty object</returns>
        /// <response code="201">Playlistsong sucessfully created</response>
        /// <response code="400">Error model</response>
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
        /// <summary>
        /// Delete a given playlistsong
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <returns>An empty object</returns>
        /// <response code="200">User sucessfully deleted</response>
        /// <response code="400">Error model</response>
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
