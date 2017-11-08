using System.Collections.Generic;

namespace Mixtape.Models
{
    public partial class Artist
    {
        public Artist()
        {
            Album = new HashSet<Album>();
            SongArtist = new HashSet<Song>();
            SongFeaturedArtist = new HashSet<Song>();
        }

        public int ArtistId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Album> Album { get; set; }
        public virtual ICollection<Song> SongArtist { get; set; }
        public virtual ICollection<Song> SongFeaturedArtist { get; set; }
    }
}
