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
        //[Authorize]
        [HttpGet]
        public IEnumerable<GlobalPlaylistSong> GetPlaylistSong()
        {
            return _context.GlobalPlaylistSong.Include(s => s.Song).Include(u => u.User);
        }


        // POST: api/GlobalPlaylistSongs
        //[Authorize]
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
