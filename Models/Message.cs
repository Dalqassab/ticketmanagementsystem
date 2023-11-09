using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagementSystem.Models
{
    public class Message
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime timestampsend { get; set; } = DateTime.UtcNow;

        [ForeignKey("Application")]
        public string? senderId { get; set; }
        public ApplicationUser sender { get; set; }

        [ForeignKey("Technician")]
        public string? TechnicianId { get; set; }
        public ApplicationUser receiver { get; set; }

        [ForeignKey("Ticket")]
        public string? TicketId { get; set; }
        public Ticket ticket { get; set; }
        

    }
}
