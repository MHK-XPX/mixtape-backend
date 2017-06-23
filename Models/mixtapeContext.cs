using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace mixtape.Models
{
    public partial class mixtapeContext : DbContext
    {
        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<AlbumRating> AlbumRating { get; set; }
        public virtual DbSet<Artist> Artist { get; set; }
        public virtual DbSet<Playlist> Playlist { get; set; }
        public virtual DbSet<PlaylistSong> PlaylistSong { get; set; }
        public virtual DbSet<Song> Song { get; set; }
        public virtual DbSet<SongRating> SongRating { get; set; }
        public virtual DbSet<User> User { get; set; }

        public mixtapeContext(DbContextOptions<mixtapeContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("ALBUM");

                entity.HasIndex(e => e.ArtistId)
                    .HasName("ARTIST_ID");

                entity.Property(e => e.AlbumId)
                    .HasColumnName("ALBUM_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ArtistId)
                    .HasColumnName("ARTIST_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Artwork)
                    .HasColumnName("ARTWORK")
                    .HasColumnType("blob");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.Year)
                    .HasColumnName("YEAR")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Album)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("ALBUM_ibfk_1");
            });

            modelBuilder.Entity<AlbumRating>(entity =>
            {
                entity.ToTable("ALBUM_RATING");

                entity.HasIndex(e => e.AlbumId)
                    .HasName("ALBUM_ID");

                entity.HasIndex(e => e.UserId)
                    .HasName("USER_ID");

                entity.Property(e => e.AlbumRatingId)
                    .HasColumnName("ALBUM_RATING_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AlbumId)
                    .HasColumnName("ALBUM_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("COMMENT")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.Rating)
                    .HasColumnName("RATING")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.AlbumRating)
                    .HasForeignKey(d => d.AlbumId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("ALBUM_RATING_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AlbumRating)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("ALBUM_RATING_ibfk_1");
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("ARTIST");

                entity.HasIndex(e => e.Name)
                    .HasName("NAME_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ArtistId)
                    .HasColumnName("ARTIST_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(250)");
            });

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.ToTable("PLAYLIST");

                entity.HasIndex(e => e.UserId)
                    .HasName("USER_ID");

                entity.Property(e => e.PlaylistId)
                    .HasColumnName("PLAYLIST_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Active)
                    .HasColumnName("ACTIVE")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Playlist)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("PLAYLIST_ibfk_1");
            });

            modelBuilder.Entity<PlaylistSong>(entity =>
            {
                entity.ToTable("PLAYLIST_SONG");

                entity.HasIndex(e => e.PlaylistId)
                    .HasName("PLAYLIST_ID");

                entity.HasIndex(e => e.SongId)
                    .HasName("SONG_ID");

                entity.Property(e => e.PlaylistSongId)
                    .HasColumnName("PLAYLIST_SONG_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PlaylistId)
                    .HasColumnName("PLAYLIST_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SongId)
                    .HasColumnName("SONG_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.PlaylistSong)
                    .HasForeignKey(d => d.PlaylistId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("PLAYLIST_SONG_ibfk_2");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.PlaylistSong)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("PLAYLIST_SONG_ibfk_1");
            });

            modelBuilder.Entity<Song>(entity =>
            {
                entity.ToTable("SONG");

                entity.HasIndex(e => e.AlbumId)
                    .HasName("ALBUM_ID");

                entity.HasIndex(e => e.ArtistId)
                    .HasName("ARTIST_ID");

                entity.HasIndex(e => e.FeaturedArtistId)
                    .HasName("FEATURED_ARTIST_ID");

                entity.Property(e => e.SongId)
                    .HasColumnName("SONG_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AlbumId)
                    .HasColumnName("ALBUM_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ArtistId)
                    .HasColumnName("ARTIST_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FeaturedArtistId)
                    .HasColumnName("FEATURED_ARTIST_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .HasColumnType("varchar(1000)");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Song)
                    .HasForeignKey(d => d.AlbumId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("SONG_ibfk_1");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.SongArtist)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("SONG_ibfk_2");

                entity.HasOne(d => d.FeaturedArtist)
                    .WithMany(p => p.SongFeaturedArtist)
                    .HasForeignKey(d => d.FeaturedArtistId)
                    .HasConstraintName("SONG_ibfk_3");
            });

            modelBuilder.Entity<SongRating>(entity =>
            {
                entity.ToTable("SONG_RATING");

                entity.HasIndex(e => e.SongId)
                    .HasName("SONG_ID");

                entity.HasIndex(e => e.UserId)
                    .HasName("USER_ID");

                entity.Property(e => e.SongRatingId)
                    .HasColumnName("SONG_RATING_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("COMMENT")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.Rating)
                    .HasColumnName("RATING")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SongId)
                    .HasColumnName("SONG_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.SongRating)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("SONG_RATING_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SongRating)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("SONG_RATING_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USER");

                entity.HasIndex(e => e.Username)
                    .HasName("USERNAME_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("FIRST_NAME")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("LAST_NAME")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("PASSWORD")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("USERNAME")
                    .HasColumnType("varchar(250)");
            });
        }
    }
}