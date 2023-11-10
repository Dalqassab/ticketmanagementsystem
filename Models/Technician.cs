using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Models
{
    public class Technician
    {
        [Key]
        [Required]
        public string TechnicianId { get; set; } 

        [Display(Name = "Area of Expertise")]
        public string areaofexpertise {  get; set; }

        [Display(Name = "Years of Experience")]
        public int yearsofexperience { get; set; }
        public virtual ICollection<Ticket> AssignedTickets { get; set; }

    }
}
