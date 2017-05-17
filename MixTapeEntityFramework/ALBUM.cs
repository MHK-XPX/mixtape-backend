namespace MixTapeEntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mixtape.ALBUM")]
    public partial class ALBUM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ALBUM()
        {
            ALBUM_RATING = new HashSet<ALBUM_RATING>();
            SONGs = new HashSet<SONG>();
        }

        [Key]
        public int ALBUM_ID { get; set; }

        [Required]
        [StringLength(250)]
        public string NAME { get; set; }

        public int YEAR { get; set; }

        [Column(TypeName = "blob")]
        public byte[] ARTWORK { get; set; }

        public int ARTIST_ID { get; set; }

        public virtual ARTIST ARTIST { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALBUM_RATING> ALBUM_RATING { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SONG> SONGs { get; set; }
    }
}
