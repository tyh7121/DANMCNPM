using CrawlerProject_API.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerProject_API.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<Conference> Conferences { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conference>(entity =>
            {
                entity.Property(e => e.Country)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("country");
                entity.Property(e => e.Date)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("date");
                entity.Property(e => e.Deadline)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("deadline");
                entity.Property(e => e.EndDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("end_date");
                entity.Property(e => e.EventStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("event_status");
                entity.Property(e => e.InquiryEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("inquiry_email");
                entity.Property(e => e.Organizer)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("organizer");
                entity.Property(e => e.RegistrationUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("registration_url");
                entity.Property(e => e.Secretary)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("secretary");
                entity.Property(e => e.StartDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("start_date");
                entity.Property(e => e.Title)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("title");
                entity.Property(e => e.Url)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("url");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
