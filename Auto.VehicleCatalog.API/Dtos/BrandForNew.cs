using System.ComponentModel.DataAnnotations;

namespace Auto.VehicleCatalog.API.Dtos
{
    public class BrandForNew
    {
        [Required]
        [MaxLength(500)]
        public string name { get; set; }
    }
}