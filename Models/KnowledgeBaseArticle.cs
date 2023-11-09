using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Models
{
    public class KnowledgeBaseArticle
    {
        [Key]
        public int ArticleID { get; set; }

        [Required, MaxLength(200)]
        public string? Title { get; set; }

        [Required]
        public string? Content { get; set; }

        [ForeignKey("Category")]
        public int TicketCategoryID { get; set; }
        public virtual Category? Category { get; set; }  // Category the article belongs to
    }
}
