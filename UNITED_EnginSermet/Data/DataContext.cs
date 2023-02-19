using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UNITED_EnginSermet.Entities;

namespace UNITED_EnginSermet.Data
{
    public class DataContext : IdentityDbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<SubNote> SubNotes { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=UnitedDB;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Note>().ToTable("Notlar");

            builder.Entity<Note>().HasMany(a => a.SubNotes)
                .WithOne(a => a.Note)
                .HasForeignKey(a => a.NoteId)
                .OnDelete(DeleteBehavior.Cascade);         
        }
    }
}