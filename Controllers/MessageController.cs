using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mixtape.Models;
using Mixtape.Models;


namespace mixtape.Controllers
{

    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessageController: Controller
    {
        private readonly DataContext _context;

        public MessageController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = new List<MessageData>();
            var messages = _context.GlobalPlaylistSong.ToList();

            foreach(GlobalPlaylistSong glps in messages)
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

        [HttpPatch]
        public IActionResult Patch([FromBody] MessageData messageData)
        {

            var glps = _context.GlobalPlaylistSong.FirstOrDefault(g => g.GlobalPlaylistSongId == messageData.GlobalPlaylistSongId);

            if(glps == null)
            {
                return NotFound();
            }

            glps.Votes = messageData.Votes;
            glps.IsStatic = messageData.IsStatic;

            _context.SaveChanges();

            return Ok(messageData);
        }

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
