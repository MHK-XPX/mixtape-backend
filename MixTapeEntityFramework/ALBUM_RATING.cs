namespace MixTapeEntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mixtape.ALBUM_RATING")]
    public partial class ALBUM_RATING
    {
        [Key]
        public int ALBUM_RATING_ID { get; set; }

        public int RATING { get; set; }

        [Required]
        [StringLength(1000)]
        public string COMMENT { get; set; }

        public int USER_ID { get; set; }

        public int ALBUM_ID { get; set; }

        public virtual ALBUM ALBUM { get; set; }

        public virtual USER USER { get; set; }
    }
}
