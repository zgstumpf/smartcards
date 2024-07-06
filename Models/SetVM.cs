namespace assignment4.Models
{
    public class SetVM
    {
        public FlashcardSet Set { get; set; }
        public SetRatingv3? Rating { get; set; }
        public ViewCount? Views { get; set; }
        public SetVM() {
            Set = new FlashcardSet();
            Rating = new SetRatingv3();
            Views = new ViewCount();
        }
    }
}
