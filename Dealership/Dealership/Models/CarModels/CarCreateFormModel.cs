namespace Dealership.Models.CarViewModels
{
    using Dealership.Data.Enums;
    using Microsoft.AspNetCore.Http.Internal;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CarCreateFormModel
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Manufacturer { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Model { get; set; }

        [Range(1950, 2100)]
        [Display(Name = "Year Of Production")]
        public short YearOfProduction { get; set; }

        [Display(Name = "Body Type")]
        public BodyType BodyType { get; set; }

        public Condition Condition { get; set; }

        [Display(Name = "Type Of Transmission")]
        public TypeOfTransmission TypeOfTransmission { get; set; }

        [Display(Name = "Euro Standart")]
        public EuroStandart EuroStandart { get; set; }

        [Display(Name = "Engine Type")]
        public EngineType EngineType { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Travelled Distance")]
        public int TravelledDistance { get; set; }

        [Range(0, short.MaxValue)]
        [Display(Name = "Horse Power")]
        public short HorsePower { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Color { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(700)]
        [Display(Name = "Description")]
        public string SaleDescription { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
