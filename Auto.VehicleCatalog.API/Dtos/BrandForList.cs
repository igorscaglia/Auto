namespace Auto.VehicleCatalog.API.Dtos
{
    // We should never expose our domain model. That's why we are using DTOs
    public class BrandForList
    {
        public string name { get; set; }

        public int id { get; set; }
    }
}