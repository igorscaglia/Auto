using Auto.VehicleCatalog.API.Model;

namespace Auto.VehicleCatalog.API.Dtos
{
    public class VehicleForDetail
    {
        public int id { get; set; }

        public string value { get; set; }

        public string brand { get; set; }

        public int brandId { get; set; }

        public string model { get; set; }    

        public int modelId { get; set; }    

        public int yearModel { get; set; }

        public string fuel { get; set; }
    }
}