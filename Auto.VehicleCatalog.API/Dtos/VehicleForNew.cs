using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.VehicleCatalog.API.Dtos
{
    public class VehicleForNew
    {
        [Required]
        public decimal value { get; set; }

        [Required]
        public int brandId { get; set; }

        [Required]
        public int modelId { get; set; }

        [Required]
        public int yearModel { get; set; }

        [Required]
        [MaxLength(20)]
        public string fuel { get; set; }
    }
}