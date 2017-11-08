using System.Collections.Generic;

namespace Mixtape.Models
{
    public partial class Playlist
    {
        public Playlist()
        {
            PlaylistSong = new HashSet<PlaylistSong>();
        }

        public int PlaylistId { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<PlaylistSong> PlaylistSong { get; set; }
        public virtual User User { get; set; }
    }
}
