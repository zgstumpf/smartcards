using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace assignment4.Models
{
    public class SetRatingv3
    {
        [Key]
        public Guid SetRatingId { get; set; }


        [ForeignKey("AspNetUsers")]
        public string? RatingUserId { get; set; }
        public IdentityUser? User { get; set; }
        public string? RatingUserName { get; set; }


        [ForeignKey("FlashcardSet")]
        public Guid? RatedSetId { get; set; }
        public FlashcardSet? FlashcardSet { get; set; }
        public string? RatedSetName { get; set; }


        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime RatedDate { get; set; }

    }
}
