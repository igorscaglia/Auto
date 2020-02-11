using System.ComponentModel.DataAnnotations;

namespace Auto.VehicleCatalog.API.Dtos
{
    public class ModelForNew
    {
        [Required]
        [MaxLength(500)]
        public string name { get; set; }

        [Required]
        public int brandId { get; set; }
    }
}