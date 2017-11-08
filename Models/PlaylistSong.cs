namespace Mixtape.Models
{
    public partial class PlaylistSong
    {
        public int PlaylistSongId { get; set; }
        public int PlaylistId { get; set; }
        public int SongId { get; set; }

        public virtual Playlist Playlist { get; set; }
        public virtual Song Song { get; set; }
    }
}
