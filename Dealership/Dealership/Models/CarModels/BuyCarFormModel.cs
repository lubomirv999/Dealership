namespace Dealership.Models.CarModels
{
    using System.ComponentModel.DataAnnotations;

    public class BuyCarFormModel
    {
        public int CarToBuyId { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "GSM for Contact")]
        public string GSM { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(800)]
        [Display(Name = "Comment (call me Mr. or call me after 8")]
        public string Comment { get; set; }
    }
}
