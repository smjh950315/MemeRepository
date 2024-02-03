using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MemeRepository.Db.Models
{
    public partial class MemeRepositoryContext : DbContext
    {
        public MemeRepositoryContext() {
        }

        public MemeRepositoryContext(DbContextOptions<MemeRepositoryContext> options)
            : base(options) {
        }

        public virtual DbSet<Category> Category { get; set; } = null!;
        public virtual DbSet<Image> Image { get; set; } = null!;
        public virtual DbSet<Tag> Tag { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
//            if (!optionsBuilder.IsConfigured) {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=localhost,1433; user id=sa; password=0000; Database=MemeRepository;integrated security=false;TrustServerCertificate=True");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Category>(entity => {
                entity.Property(e => e.CategoryName).HasMaxLength(100);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Image>(entity => {
                entity.Property(e => e.ImageID).HasComment("圖片ID");

                entity.Property(e => e.CategoryID).HasComment("圖片分類ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.ImageData).HasComment("圖片資料");

                entity.Property(e => e.ImageSize).HasComment("圖片大小");

                entity.Property(e => e.ImageType)
                    .HasMaxLength(50)
                    .HasComment("圖片類型");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Tag>(entity => {
                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
