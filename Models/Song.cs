using System;
using System.Collections.Generic;

namespace mixtape.Models
{
    public partial class Song
    {
        public Song()
        {
            PlaylistSong = new HashSet<PlaylistSong>();
            SongRating = new HashSet<SongRating>();
        }

        public int SongId { get; set; }
        public int AlbumId { get; set; }
        public int ArtistId { get; set; }
        public int? FeaturedArtistId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual ICollection<PlaylistSong> PlaylistSong { get; set; }
        public virtual ICollection<SongRating> SongRating { get; set; }
        public virtual Album Album { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual Artist FeaturedArtist { get; set; }
    }
}
