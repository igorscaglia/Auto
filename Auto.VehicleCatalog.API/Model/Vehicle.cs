namespace Auto.VehicleCatalog.API.Model
{
    public class Vehicle
    {
        public int Id { get; set; }

        public decimal Value { get; set; }

        public virtual Brand Brand { get; set; }

        public int BrandId { get; set; }

        public virtual Model Model { get; set; }    

        public int ModelId { get; set; }

        public int YearModel { get; set; }

        public string Fuel { get; set; }
    }
}
