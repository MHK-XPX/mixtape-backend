namespace MixTapeEntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mixtape.USER")]
    public partial class USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER()
        {
            ALBUM_RATING = new HashSet<ALBUM_RATING>();
            PLAYLISTs = new HashSet<PLAYLIST>();
            SONG_RATING = new HashSet<SONG_RATING>();
        }

        [Key]
        public int USER_ID { get; set; }

        [Required]
        [StringLength(250)]
        public string USERNAME { get; set; }

        [Required]
        [StringLength(250)]
        public string PASSWORD { get; set; }

        [Required]
        [StringLength(250)]
        public string FIRST_NAME { get; set; }

        [Required]
        [StringLength(250)]
        public string LAST_NAME { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALBUM_RATING> ALBUM_RATING { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PLAYLIST> PLAYLISTs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SONG_RATING> SONG_RATING { get; set; }
    }
}
