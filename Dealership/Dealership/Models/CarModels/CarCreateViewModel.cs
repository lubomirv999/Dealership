namespace Dealership.Models.CarViewModels
{
    using Dealership.Data.Enums;
    using System.ComponentModel.DataAnnotations;

    public class CarCreateViewModel
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
        public short YearOfProduction { get; set; }

        public BodyType BodyType { get; set; }

        public Condition Condition { get; set; }

        public TypeOfTransmission TypeOfTransmission { get; set; }

        public EuroStandart EuroStandart { get; set; }

        public EngineType EngineType { get; set; }

        [Range(0, int.MaxValue)]
        public int TravelledDistance { get; set; }

        [Range(0, short.MaxValue)]
        public short HorsePower { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Color { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(700)]
        public string SaleDescription { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
