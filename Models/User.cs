using System.Collections.Generic;

namespace Mixtape.Models
{
    public partial class User
    {
        public User()
        {
            AlbumRating = new HashSet<AlbumRating>();
            Playlist = new HashSet<Playlist>();
            SongRating = new HashSet<SongRating>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }

        public virtual ICollection<AlbumRating> AlbumRating { get; set; }
        public virtual ICollection<Playlist> Playlist { get; set; }
        public virtual ICollection<SongRating> SongRating { get; set; }
    }
}
