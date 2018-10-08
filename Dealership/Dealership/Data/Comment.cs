namespace Dealership.Data
{
    using Dealership.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(500)]
        public string Content { get; set; }
        
        public int? ParentCommentId { get; set; }
        
        public Comment ParentComment { get; set; }
        
        public int CarId { get; set; }
        
        public Car Car { get; set; }

        public string UserId { get; set; }
        
        public ApplicationUser Author { get; set; }
    }
}
