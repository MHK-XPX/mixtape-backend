namespace MixTapeEntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mixtape.SONG")]
    public partial class SONG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SONG()
        {
            PLAYLIST_SONG = new HashSet<PLAYLIST_SONG>();
            SONG_RATING = new HashSet<SONG_RATING>();
        }

        [Key]
        public int SONG_ID { get; set; }

        [Required]
        [StringLength(250)]
        public string NAME { get; set; }

        [Required]
        [StringLength(1000)]
        public string URL { get; set; }

        public int ALBUM_ID { get; set; }

        public int ARTIST_ID { get; set; }

        public int? FEATURED_ARTIST_ID { get; set; }

        public virtual ALBUM ALBUM { get; set; }

        public virtual ARTIST ARTIST { get; set; }

        public virtual ARTIST ARTIST1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PLAYLIST_SONG> PLAYLIST_SONG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SONG_RATING> SONG_RATING { get; set; }
    }
}
