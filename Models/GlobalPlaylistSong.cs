namespace Mixtape.Models
{
    public partial class GlobalPlaylistSong
    {
        public int GlobalPlaylistSongId { get; set; }
        public int SongId { get; set; }
        public int UserId { get; set; }
        public int Votes { get; set; }
        public bool IsStatic { get; set; }

        public virtual Song Song { get; set; }
        public virtual User User { get; set; }
    }
}
