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
        public DateTime Timestampsend { get; set; } = DateTime.UtcNow;

        [ForeignKey("Ticket")]
        public string? TicketId { get; set; }
        public Ticket? TheTicket { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? SenderId { get; set; }
        public ApplicationUser? Sender { get; set; }

        [ForeignKey("ApplicationUser")]
        [Display(Name = "Send to:(email)")]
        public string? ReceiverID { get; set; }
        public ApplicationUser? Receiver { get; set; }

        // New properties for self-referencing relationship
        public int? ParentMessageId { get; set; } // Nullable for top-level messages
        public virtual Message? ParentMessage { get; set; } // Navigation property to the parent message

        public virtual ICollection<Message> Replies { get; set; } = new List<Message>(); // Collection of replies


    }
}
