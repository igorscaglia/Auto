using System.ComponentModel.DataAnnotations;

namespace Auto.VehicleCatalog.API.Dtos
{
    public class VehicleForUpdate
    {
        [Required]
        public decimal value { get; set; }

        public int? brandId { get; set; }

        public int? modelId { get; set; }

        [Required]
        public int yearModel { get; set; }

        [Required]
        [MaxLength(20)]
        public string fuel { get; set; }
    }
}