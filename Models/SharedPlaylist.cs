using System.Collections.Generic;

namespace Mixtape.Models
{
    public partial class SharedPlaylist
    {
        public SharedPlaylist()
        {
        }

        public int SharedPlaylistId { get; set; }
        public string Owner { get; set; }
        public int SharedWith { get; set; }
        public int PlaylistId { get; set; }

        public virtual User User { get; set; }
        public virtual Playlist Playlist { get; set; }
    }
}
