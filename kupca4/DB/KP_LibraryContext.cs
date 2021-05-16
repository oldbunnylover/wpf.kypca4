using Microsoft.EntityFrameworkCore;

#nullable disable

namespace kupca4.DB
{
    public partial class KP_LibraryContext : DbContext
    {
        public KP_LibraryContext()
        {
            Database.EnsureCreated();
        }

        public KP_LibraryContext(DbContextOptions<KP_LibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<SavedBook> SavedBooks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<BookRates> BookRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:courseprojectformsuicservice.database.windows.net,1433;Initial Catalog=KP_Library;Persist Security Info=False;User ID=NIikitoza;Password=140410qq!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=5;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Book>(entity =>
            {

                entity.Property(e => e.BookId).HasColumnName("bookID");

                entity.Property(e => e.Applied)
                    .HasColumnName("applied")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Hidden)
                    .HasColumnName("hidden")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Bookname)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("bookname");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasColumnName("description");

                entity.Property(e => e.GenreId).HasColumnName("genreID")
                    .IsRequired();

                entity.Property(e => e.AuthorName)
                    .HasMaxLength(255)
                    .HasColumnName("authorName")
                    .IsRequired();

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK__Books__genreID__02FC7413");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorName)
                    .HasConstraintName("FK__Books__AuthorName__02FC7413");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.GenreId).HasColumnName("genreID");

                entity.Property(e => e.Genrename)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("genrename");
            });

            modelBuilder.Entity<SavedBook>(entity =>
            {
                entity.HasKey(e => new { e.Username, e.BookId })
                    .HasName("PK__SavedBoo__3B659F619AA05702");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("username");

                entity.Property(e => e.BookId).HasColumnName("bookID");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.SavedBooks)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SavedBook__bookI__06CD04F7");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.SavedBooks)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SavedBook__usern__05D8E0BE");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__Users__F3DBC5736C45C8B3");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("username");

                entity.Property(e => e.Blocked)
                    .HasColumnName("blocked")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("fullname");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<BookRates>(entity =>
            {
                entity.HasKey(e => new { e.Username, e.BookId })
                    .HasName("PK__BookRates__3B659F619AA05702");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("username");

                entity.Property(e => e.BookId).HasColumnName("bookID");

                entity.Property(e => e.UserRate).HasColumnName("userRate");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookRates)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookRates__bookI__06CD04F7");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.BookRates)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookRates__usern__05D8E0BE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
