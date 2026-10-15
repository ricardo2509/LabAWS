using LabAws.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAws.Infrastructure.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<UploadLog> UploadLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UploadLog>(entity =>
            {
                entity.ToTable("upload_logs");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.FileName)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.ContentType)
                         .HasMaxLength(100);

                entity.Property(e => e.BucketName)
                      .HasMaxLength(63)
                      .IsRequired();

                entity.Property(e => e.S3Url)
                      .HasMaxLength(2048)
                      .IsRequired();
            });
        }
    }
}
