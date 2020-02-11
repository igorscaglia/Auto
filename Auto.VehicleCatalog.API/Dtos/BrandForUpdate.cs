using System.ComponentModel.DataAnnotations;

namespace Auto.VehicleCatalog.API.Dtos
{
    public class BrandForUpdate
    {
        [Required]
        [MaxLength(500)]
        public string name { get; set; }
    }
}