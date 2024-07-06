using System.ComponentModel.DataAnnotations.Schema;
using System.Composition;

namespace assignment4.Models
{
    public class Report
    {
        public Guid ReportId { get; set; }
        [ForeignKey("SetId")]
        public Guid SetId { get; set; }
        public FlashcardSet Set { get; set; }
        public List<SetRatingv3>? Ratings { get; set; }
        [NotMapped]
        public string? AverageRating { get; set; }
        [ForeignKey("ViewCountId")]
        public Guid? ViewCountId { get; set; }
        public ViewCount? ViewCount { get; set; }
        /* 
            Q: Why is there a foreign key constraint for ViewCountId, but not for SetRatingv3?
            A: There are multiple ratings per set, but only one ViewCount per set. So our LINQ
            will just collect a list of Ratings based on the SetId.
            - Sid
        */
        public void CalculateAverageRating()
        {
            var rating = Ratings
                    .Where(r => r.RatedSetId == SetId)
                    .Average(r => r.Rating);
            if (rating != null)
            {
                // This abomination gets the average rating (double), rounds it up or down, converts to a string, then converts to an int
                AverageRating = Math.Round(rating.Value, MidpointRounding.ToEven).ToString();
            }
        }
    }
}
