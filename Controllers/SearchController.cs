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
    [Route("api/Search")]
    public class SearchController : Controller
    {
        private readonly DataContext _context;

        public SearchController(DataContext context)
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
        //[ProducesResponseType(typeof(ICollection<PlaylistSong>), 200)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(200, Type = typeof(PlaylistSong), Description = "Playlistsong objects returned successfully")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
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
        [HttpGet("{search}")]
        //[ProducesResponseType(typeof(PlaylistSong), 200)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(200, Type = typeof(PlaylistSong), Description = "Playlistsong object returned successfully")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
        public async Task<IActionResult> GetPlaylistSong([FromRoute] string search)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /*
             * First:
             *  Check to see if there are artists
             *      If so, we will want to return them
             *  Check to see if there are songs:
             *      If so do the following:
             *          1) If they match the artist --> return just the artists songs first
             *          2) If thy do not match the artist --> just return the songs
             */
            var sr = new SearchResults();

            sr.Artists = await _context.Artist.Where(art => art.Name.Contains(search)).AsNoTracking().ToListAsync();
            sr.Albums = await _context.Album.Where(alb => alb.Name.Contains(search)).AsNoTracking().ToListAsync();
            sr.Songs = await _context.Song.Where(s => s.Name.Contains(search)).AsNoTracking().ToListAsync();

            //sr.Artists = await _context.Artist.ToListAsync(art => art.Name.Contains(search));

            if(sr.Artists.Count > 0)
            {
                if(sr.Albums.Count == 0 && sr.Songs.Count == 0)
                {
                    int artId = sr.Artists.ElementAt(0).ArtistId;
                    sr.Albums = await _context.Album.Where(alb => alb.ArtistId == artId).AsNoTracking().ToListAsync();
                    sr.Songs = await _context.Song.Where(s => s.ArtistId == artId).AsNoTracking().ToListAsync();
                }
            }else if(sr.Albums.Count > 0)
            {
                foreach(Album alb in sr.Albums)
                {
                    sr.Artists = await _context.Artist.Where(art => art.ArtistId == alb.ArtistId).AsNoTracking().ToListAsync();
                    sr.Songs = await _context.Song.Where(s => s.AlbumId == alb.AlbumId).AsNoTracking().ToListAsync();
                }
            }

            if (sr == null)
            {
                return NotFound();
            }

            return Ok(sr);
        }

        private class SearchResults
        {

            public ICollection<Artist> Artists { get; set; }
            public ICollection<Album> Albums { get; set; }
            public ICollection<Song> Songs { get; set; }
        }
    }
}
