using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace assignment4.Models
{
    public class Flashcard
    {
        [Key]
        public Guid FlashcardId { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }

        // One-to-many relationship with Set
        [ForeignKey("FlashcardSet")]
        public Guid? SetId { get; set; }
        public FlashcardSet? FlashcardSet { get; set; }

        [ForeignKey("AspNetUsers")]
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Flashcard()
        {
            CreatedDate = DateTime.Now;
            IsDeleted = false;
            
        }
    }
}
