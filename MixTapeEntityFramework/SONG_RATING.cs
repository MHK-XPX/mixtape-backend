namespace MixTapeEntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mixtape.SONG_RATING")]
    public partial class SONG_RATING
    {
        [Key]
        public int SONG_RATING_ID { get; set; }

        public int RATING { get; set; }

        [Required]
        [StringLength(1000)]
        public string COMMENT { get; set; }

        public int USER_ID { get; set; }

        public int SONG_ID { get; set; }

        public virtual SONG SONG { get; set; }

        public virtual USER USER { get; set; }
    }
}
