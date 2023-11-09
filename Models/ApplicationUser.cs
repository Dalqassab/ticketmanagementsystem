using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; internal set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string LastName { get; internal set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Address cannot be longer than 100 characters.")]
        public string Address { get; internal set; }
    }
}
