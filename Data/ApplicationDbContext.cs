using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using assignment4.Models;

namespace assignment4.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<assignment4.Models.Flashcard>? Flashcard { get; set; }
        public DbSet<assignment4.Models.FlashcardSet>? FlashcardSet { get; set; }
        public DbSet<assignment4.Models.ProjectRole>? ProjectRole { get; set; }
        public DbSet<assignment4.Models.PromotionRequest>? PromotionRequest { get; set; }
        public DbSet<assignment4.Models.ViewCount>? ViewCount { get; set; }
        public DbSet<assignment4.Models.SetRatingv3>? SetRatingv3 { get; set; }
        public DbSet<assignment4.Models.Report>? Report { get; set; }
    }
}