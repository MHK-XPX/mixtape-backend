using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Mixtape.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.AspNetCore.Authorization;

namespace Mixtape.Controllers
{
    [Produces("application/json")]
    [Route("api/GlobalPlaylistSongs")]
    public class GlobalPlaylistSongsController : Controller
    {
        private readonly DataContext _context;

        public GlobalPlaylistSongsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/GlobalPlaylistSongs
        /// <summary>
        /// Gets all global playlist songs
        /// </summary>
        /// <returns></returns>
        /// <response code="200">A list of global playlist songs</response>
        [HttpGet]
        public IEnumerable<GlobalPlaylistSong> GetPlaylistSong()
        {
            return _context.GlobalPlaylistSong.Include(s => s.Song).Include(u => u.User);
        }


        // POST: api/GlobalPlaylistSongs
        /// <summary>
        /// Adds a global playlist song to the database
        /// </summary>
        /// <param name="globalPlaylistSong">The global playlist song to add</param>
        /// <returns></returns>
        /// <response code="200">The added global playlist song object</response>
        [HttpPost]
        public async Task<IActionResult> PostPlaylistSong([FromBody] GlobalPlaylistSong globalPlaylistSong)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GlobalPlaylistSong.Add(globalPlaylistSong);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlaylistSong", new { id = globalPlaylistSong.GlobalPlaylistSongId }, globalPlaylistSong);
        }

        private bool GlobalPlaylistSongExists(int id)
        {
            return _context.GlobalPlaylistSong.Any(e => e.GlobalPlaylistSongId == id);
        }
    }
}
