using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MemeRepository.Db.Models
{
    public partial class MemeRepositoryContext : DbContext
    {
        public MemeRepositoryContext()
        {
        }

        public MemeRepositoryContext(DbContextOptions<MemeRepositoryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; } = null!;
        public virtual DbSet<CategoryDetail> CategoryDetail { get; set; } = null!;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CategoryID).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<CategoryDetail>(entity =>
            {
                entity.HasKey(e => e.CategoryName);

                entity.Property(e => e.CategoryName).HasMaxLength(128);

                entity.Property(e => e.CDID).ValueGeneratedOnAdd();

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<IMAGE>(entity =>
            {
                entity.Property(e => e.CREATED)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NAME).HasMaxLength(128);

                entity.Property(e => e.TYPE).HasMaxLength(50);

                entity.Property(e => e.UPDATED).HasColumnType("datetime");
            });

            modelBuilder.Entity<TAG>(entity =>
            {
                entity.Property(e => e.CREATED).HasColumnType("datetime");

                entity.Property(e => e.NAME).HasMaxLength(256);

                entity.Property(e => e.UPDATED).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
