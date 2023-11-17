using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace TicketManagementSystem.Models
{
    [Authorize]
    public class Ticket
    {

        [Key]
        public int TicketID { get; set; }

        [Required, MaxLength(200)]
        public string? Subject { get; set; }

        [Required, MaxLength(2000)]
        public string? Description { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; } = DateTime.UtcNow;

        public enum PriorityLevel { Low, Medium, High }
        [Required]
        public PriorityLevel Priority { get; set; }

        public enum TicketStatus { Open, InProgress, Resolved, Closed }
        [Required]
        public TicketStatus Status { get; set; }

        [ForeignKey("User")]
        public string? Id { get; set; }
        [Required]  // Indicates that a Ticket must have a User.
        public virtual ApplicationUser? User { get; set; }  // User who created the ticket

        [ForeignKey("Category")]
        public int TicketCategoryID { get; set; }
        public virtual Category? Category { get; set; }  // This assumes that you'll have a Category model.
    }
}
