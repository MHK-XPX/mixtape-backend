namespace MixTapeEntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mixtape.PLAYLIST")]
    public partial class PLAYLIST
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PLAYLIST()
        {
            PLAYLIST_SONG = new HashSet<PLAYLIST_SONG>();
        }

        [Key]
        public int PLAYLIST_ID { get; set; }

        [Required]
        [StringLength(1000)]
        public string NAME { get; set; }

        [Column(TypeName = "bit")]
        public bool ACTIVE { get; set; }

        public int USER_ID { get; set; }

        public virtual USER USER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PLAYLIST_SONG> PLAYLIST_SONG { get; set; }
    }
}
