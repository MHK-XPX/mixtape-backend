namespace MixTapeEntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mixtape.PLAYLIST_SONG")]
    public partial class PLAYLIST_SONG
    {
        [Key]
        public int PLAYLIST_SONG_ID { get; set; }

        public int SONG_ID { get; set; }

        public int PLAYLIST_ID { get; set; }

        public virtual PLAYLIST PLAYLIST { get; set; }

        public virtual SONG SONG { get; set; }
    }
}
