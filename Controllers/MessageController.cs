using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mixtape.Models;

namespace Mixtape.Controllers
{

    [Produces("application/json")]
    [Route("api/Messages")]
    [Authorize]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly DataContext _context;

        public MessageController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all message data objects
        /// </summary>
        /// <returns></returns>
        /// <response code="200">A list of all message data objects</response>
        [HttpGet]
        public IActionResult Get()
        {
            var list = new List<MessageData>();
            var messages = _context.GlobalPlaylistSong.ToList();

            foreach (GlobalPlaylistSong glps in messages)
            {
                var user = _context.User.SingleOrDefault(x => x.UserId == glps.UserId);
                var song = _context.Song.SingleOrDefault(x => x.SongId == glps.SongId);

                list.Add(
                    new MessageData
                    {
                        GlobalPlaylistSongId = glps.GlobalPlaylistSongId,
                        Username = user.Username,
                        Song = song,
                        Votes = glps.Votes,
                        IsStatic = glps.IsStatic
                    }
                );
            }

            return Ok(list);
        }

        /// <summary>
        /// Updates a given message data object
        /// </summary>
        /// <param name="messageData">The updated message data object</param>
        /// <returns></returns>
        /// <response code="200">No content</response>
        [HttpPatch]
        public IActionResult Patch([FromBody] MessageData messageData)
        {

            var glps = _context.GlobalPlaylistSong.FirstOrDefault(g => g.GlobalPlaylistSongId == messageData.GlobalPlaylistSongId);

            if (glps == null)
            {
                return NotFound();
            }

            glps.Votes = messageData.Votes;
            glps.IsStatic = messageData.IsStatic;

            _context.SaveChanges();

            return Ok(messageData);
        }

        /// <summary>
        /// Removes a specific message data object from the database
        /// </summary>
        /// <param name="id">The id of the message data object to remove</param>
        /// <returns></returns>
        /// <response code="200">The removed message data object</response>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var glps = _context.GlobalPlaylistSong.FirstOrDefault(g => g.GlobalPlaylistSongId == id);

            if (glps == null)
            {
                return NotFound();
            }

            _context.GlobalPlaylistSong.Remove(glps);

            _context.SaveChanges();

            return this.Get();
        }

    }
}
