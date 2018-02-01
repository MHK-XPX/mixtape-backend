using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Mixtape.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Mixtape.Controllers
{
    [Produces("application/json")]
    [Route("api/Playlists")]
    public class PlaylistsController : Controller
    {
        private readonly DataContext _context;

        public PlaylistsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Playlists
        /// <summary>
        /// Get all playlists
        /// </summary>
        /// <returns>All playlist entities</returns>
        /// <response code="200">Playlist entities</response>
        /// <response code="400">Error model</response>
        [HttpGet]
        //[ProducesResponseType(typeof(ICollection<Playlist>), 200)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(200, Type = typeof(Playlist), Description = "Playist objects returned successfully")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
        public IEnumerable<Playlist> GetPlaylist()
        {
            return _context.Playlist;
        }

        // GET: api/Playlists/5
        /// <summary>
        /// Get a given playlist
        /// </summary>
        /// <param name="id">ID of the playlist</param>
        /// <returns>A given playlist entity</returns>
        /// <response code="200">Playlist entity</response>
        /// <response code="400">Error model</response>
        [HttpGet("{id}")]
        //[ProducesResponseType(typeof(Playlist), 200)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(200, Type = typeof(Playlist), Description = "Playlist object returned successfully")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
        public async Task<IActionResult> GetPlaylist([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playlist = await _context.Playlist
                                        .Include(pls => pls.PlaylistSong)
                                            .ThenInclude(s => s.Song)
                                        .AsNoTracking() //Read only so we do not need to save it => AsNoTracking
                                        .SingleOrDefaultAsync(m => m.PlaylistId == id);

            if (playlist == null)
            {
                return NotFound();
            }

            return Ok(playlist);
        }

        // PUT: api/Playlists/5
        /// <summary>
        /// Updates a given playlist
        /// </summary>
        /// <param name="id">ID of the playlist</param>
        /// <param name="playlist">The updated playlist entity</param>
        /// <returns>An empty object</returns>
        /// <response code="200">Playlist sucessfully updated</response>
        /// <response code="400">Error model</response>
        [HttpPut("{id}")]
        //[ProducesResponseType(typeof(Playlist), 200)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(200, Type = typeof(Playlist), Description = "Playlist updated successfully")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
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
        /// <summary>
        /// Creates a new playlist
        /// </summary>
        /// <param name="playlist">The new playlist to post</param>
        /// <returns>An empty object</returns>
        /// <response code="201">Playlist sucessfully created</response>
        /// <response code="400">Error model</response>
        [HttpPost]
        //[ProducesResponseType(typeof(Playlist), 201)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(201, Type = typeof(Playlist), Description = "Playlist successfully created")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
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
        /// <summary>
        /// Delete a given playlist
        /// </summary>
        /// <param name="id">ID of the playlist</param>
        /// <returns>An empty object</returns>
        /// <response code="200">Playlist sucessfully deleted</response>
        /// <response code="400">Error model</response>
        [HttpDelete("{id}")]
        //[ProducesResponseType(typeof(Playlist), 200)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(200, Type = typeof(Playlist), Description = "Playlist deleted successfully")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
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

            //If the playlist contains playlist songs, we must delete them first
            var plsList = await _context.PlaylistSong
                            .Where(m => m.PlaylistId == playlist.PlaylistId)
                            .AsNoTracking().ToListAsync();

            if(plsList != null || plsList.Count > 0)
            {
                foreach(PlaylistSong pls in plsList)
                {
                    _context.PlaylistSong.Remove(pls);
                }
            }

            //once the playlist songs are deleted we can delete the playlist
            _context.Playlist.Remove(playlist);
            await _context.SaveChangesAsync();

            return Ok(playlist);
        }


        // GET: api/Playlists/User/5
        /// <summary>
        /// Get all playlists for a given user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>All the playlists for a given user</returns>
        /// <response code="200">Playlist entities</response>
        /// <response code="400">Error model</response>
        [Authorize]
        [HttpGet("User/{userId}")]
        //[ProducesResponseType(typeof(ICollection<Playlist>), 200)]
        //[ProducesResponseType(typeof(Error), 400)]
        //[SwaggerResponse(200, Type = typeof(ICollection<Playlist>), Description = "Playlist objects returned successfully")]
        //[SwaggerResponse(400, Type = typeof(Error), Description = "Bad Request")]
        public async Task<IActionResult> GetPlaylistGivenUser([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playlists = await _context.Playlist
                                    .Include(pls => pls.PlaylistSong)
                                           .ThenInclude(s => s.Song)
                                    .Where(m => m.UserId == userId)
                                    .AsNoTracking().ToListAsync();

            if (playlists == null)
            {
                return NotFound();
            }

            return Ok(playlists);
        }

        private bool PlaylistExists(int id)
        {
            return _context.Playlist.Any(e => e.PlaylistId == id);
        }
    }
}
