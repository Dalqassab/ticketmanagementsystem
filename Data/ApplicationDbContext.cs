using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TicketManagementSystem.Models.Category> Category { get; set; } = default!;
        public DbSet<TicketManagementSystem.Models.Ticket> Ticket { get; set; } = default!;
        public DbSet<TicketManagementSystem.Models.KnowledgeBaseArticle> KnowledgeBaseArticle { get; set; } = default!;
        
    }
}