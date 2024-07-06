using System.ComponentModel.DataAnnotations.Schema;

namespace assignment4.Models
{
    public class ViewCount
    {
        public Guid ViewCountId { get; set; }
        [ForeignKey("SetId")]
        public Guid SetId { get; set; }
        public FlashcardSet? Set { get; set; }
        public int Count { get; set; }
    }
    public class BrowserView
    {
        public Guid setId { get; set; }
    }
}
