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

        // GET: api/Search
        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">No Content</response>
        [HttpGet]
        public IActionResult GetPlaylistSong()
        {
            return NoContent();
        }

        // GET: api/Search/Garth Brooks
        /// <summary>
        /// Get any artist, albums, or songs that match a given search string
        /// </summary>
        /// <param name="search">The string to search for</param>
        /// <returns></returns>
        /// <response code="200">Artist, Album, and/or songs matching the search string</response>
        /// <response code="400">Error model</response>
        [HttpGet("{search}")]
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
            sr.Albums = await _context.Album.Where(alb => alb.Name.Contains(search)).Include(a => a.Artist).AsNoTracking().ToListAsync();
            sr.Songs = await _context.Song.Where(s => s.Name.Contains(search)).Include(a => a.Artist).AsNoTracking().ToListAsync();

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
