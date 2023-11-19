using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagementSystem.Models
{
    public class Message
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Subject { get; set; }
       
        [Required, MaxLength(2000)]
        [Display(Name = "Message")]
        public string Body { get; set; }
        [Required]
        public DateTime timestampsend { get; set; } = DateTime.UtcNow;

        [ForeignKey("ApplicationUser")]
        public string? senderId { get; set; }
        public ApplicationUser sender { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? receiverID { get; set; }
        public ApplicationUser receiver { get; set; }

             

    }
}
