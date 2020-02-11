using System.ComponentModel.DataAnnotations;

namespace Auto.VehicleCatalog.API.Dtos
{
    public class ModelForUpdate
    {
        [Required]
        [MaxLength(500)]
        public string name { get; set; }

        public int? brandId { get; set; }
    }
}