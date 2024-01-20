using Microsoft.EntityFrameworkCore;

namespace MemeRepository.Db.Models
{
    public partial class MemeRepositoryContext : DbContext
    {
        public MemeRepositoryContext() {
        }

        public MemeRepositoryContext(DbContextOptions<MemeRepositoryContext> options)
            : base(options) {
        }

        public virtual DbSet<IMAGE> IMAGE { get; set; } = null!;
        public virtual DbSet<TAG> TAG { get; set; } = null!;
        public virtual DbSet<TAG_BINDING> TAG_BINDING { get; set; } = null!;

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Server=localhost,1433; user id=sa; password=0000; Database=MemeRepository;integrated security=false;TrustServerCertificate=True");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<IMAGE>(entity => {
                entity.Property(e => e.DATA)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.REMIND).HasMaxLength(1024);

                entity.Property(e => e.TITLE).HasMaxLength(128);

                entity.Property(e => e.UPLOADED).HasColumnType("datetime");
            });

            modelBuilder.Entity<TAG>(entity => {
                entity.Property(e => e.CREATED).HasColumnType("datetime");

                entity.Property(e => e.NAME).HasMaxLength(128);

                entity.Property(e => e.REMIND).HasMaxLength(1024);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
