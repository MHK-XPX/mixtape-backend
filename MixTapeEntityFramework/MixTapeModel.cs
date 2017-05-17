namespace MixTapeEntityFramework
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MixTapeModel : DbContext
    {
        public MixTapeModel()
            : base("name=MixTapeContext")
        {
        }

        public virtual DbSet<ALBUM> ALBUMs { get; set; }
        public virtual DbSet<ALBUM_RATING> ALBUM_RATING { get; set; }
        public virtual DbSet<ARTIST> ARTISTs { get; set; }
        public virtual DbSet<PLAYLIST> PLAYLISTs { get; set; }
        public virtual DbSet<PLAYLIST_SONG> PLAYLIST_SONG { get; set; }
        public virtual DbSet<SONG> SONGs { get; set; }
        public virtual DbSet<SONG_RATING> SONG_RATING { get; set; }
        public virtual DbSet<USER> USERs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ALBUM>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<ALBUM>()
                .HasMany(e => e.ALBUM_RATING)
                .WithRequired(e => e.ALBUM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ALBUM>()
                .HasMany(e => e.SONGs)
                .WithRequired(e => e.ALBUM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ALBUM_RATING>()
                .Property(e => e.COMMENT)
                .IsUnicode(false);

            modelBuilder.Entity<ARTIST>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<ARTIST>()
                .HasMany(e => e.ALBUMs)
                .WithRequired(e => e.ARTIST)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARTIST>()
                .HasMany(e => e.SONGs)
                .WithRequired(e => e.ARTIST)
                .HasForeignKey(e => e.ARTIST_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ARTIST>()
                .HasMany(e => e.SONGs1)
                .WithOptional(e => e.ARTIST1)
                .HasForeignKey(e => e.FEATURED_ARTIST_ID);

            modelBuilder.Entity<PLAYLIST>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<PLAYLIST>()
                .HasMany(e => e.PLAYLIST_SONG)
                .WithRequired(e => e.PLAYLIST)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SONG>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<SONG>()
                .Property(e => e.URL)
                .IsUnicode(false);

            modelBuilder.Entity<SONG>()
                .HasMany(e => e.PLAYLIST_SONG)
                .WithRequired(e => e.SONG)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SONG>()
                .HasMany(e => e.SONG_RATING)
                .WithRequired(e => e.SONG)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SONG_RATING>()
                .Property(e => e.COMMENT)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.USERNAME)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.FIRST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.LAST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.ALBUM_RATING)
                .WithRequired(e => e.USER)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.PLAYLISTs)
                .WithRequired(e => e.USER)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.SONG_RATING)
                .WithRequired(e => e.USER)
                .WillCascadeOnDelete(false);
        }
    }
}
