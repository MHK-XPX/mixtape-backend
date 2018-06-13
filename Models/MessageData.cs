using Mixtape.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mixtape.Models
{
    public class MessageData
    {
        public int GlobalPlaylistSongId { get; set; }
        public string Username { get; set; }
        public Song Song { get; set; }
        public int Votes { get; set; }
        public bool IsStatic { get; set; }
    }
}
