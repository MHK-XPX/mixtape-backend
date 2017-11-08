namespace Mixtape.Models
{
    public partial class AlbumRating
    {
        public int AlbumRatingId { get; set; }
        public int AlbumId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }

        public virtual Album Album { get; set; }
        public virtual User User { get; set; }
    }
}
