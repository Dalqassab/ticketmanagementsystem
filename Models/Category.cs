using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required, MaxLength(100)]
        public string? CategoryName { get; set; }

        // Navigation property for tickets under this category
        public virtual ICollection<Ticket>? Tickets { get; set; }

    }
}
