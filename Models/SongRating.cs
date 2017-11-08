namespace Mixtape.Models
{
    public partial class SongRating
    {
        public int SongRatingId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int SongId { get; set; }
        public int UserId { get; set; }

        public virtual Song Song { get; set; }
        public virtual User User { get; set; }
    }
}
