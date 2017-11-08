using System.Collections.Generic;

namespace Mixtape.Models
{
    public partial class Album
    {
        public Album()
        {
            AlbumRating = new HashSet<AlbumRating>();
            Song = new HashSet<Song>();
        }

        public int AlbumId { get; set; }
        public int ArtistId { get; set; }
        public byte[] Artwork { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }

        public virtual ICollection<AlbumRating> AlbumRating { get; set; }
        public virtual ICollection<Song> Song { get; set; }
        public virtual Artist Artist { get; set; }
    }
}
