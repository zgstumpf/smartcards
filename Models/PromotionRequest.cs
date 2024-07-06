using System.ComponentModel.DataAnnotations;

namespace assignment4.Models
{
    public class PromotionRequest
    {
        [Key]
        public Guid PromotionRequestId { get; set; }
        public Guid? RequesterUserId { get; set; }    
        public string? RequesterUserName { get; set; }
        public DateTime? RequestedDate { get; set; }
        public string? RequestDetails { get; set; }
        public bool? WasPromotionGranted { get; set; }
        public DateTime? GrantedDate { get; set; }
        public Guid? GrantedById { get; set; }
        public string? GrantedByName { get; set; }


    }
}
