using Microsoft.EntityFrameworkCore;
using KdxMigrationAPI.Models;

namespace KdxMigrationAPI.Data
{
    /// <summary>
    /// PostgreSQL用のDbContext
    /// </summary>
    public class MigrationDbContext : DbContext
    {
        public MigrationDbContext(DbContextOptions<MigrationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// IOテーブル
        /// </summary>
        public DbSet<IO> IOs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // IOテーブルの複合主キー設定
            modelBuilder.Entity<IO>()
                .HasKey(io => new { io.Address, io.PlcId });

            // インデックス設定
            modelBuilder.Entity<IO>()
                .HasIndex(io => io.IOType)
                .HasDatabaseName("IX_IO_IOType");

            modelBuilder.Entity<IO>()
                .HasIndex(io => io.IsEnabled)
                .HasDatabaseName("IX_IO_IsEnabled");

            // デフォルト値設定
            modelBuilder.Entity<IO>()
                .Property(io => io.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<IO>()
                .Property(io => io.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}