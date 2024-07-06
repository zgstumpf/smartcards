using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace assignment4.Models
{
    public class FlashcardSet
    {
        [Key]
        public Guid SetId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        [ForeignKey("AspNetUsers")]
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<Flashcard>? Flashcards { get; set; }
        public FlashcardSet()
        {
            CreatedDate = DateTime.Now;
            IsDeleted = false;
        }
        public List<int> RandomCards() {
            int count = Flashcards.Count;
            int max = count;
            int min = 0;
            Random random = new();
            HashSet<int> set = new();

            while (set.Count < count)
            {
                set.Add(random.Next(min, max));
            }
            
            return set.ToList();
        }
    }
}
