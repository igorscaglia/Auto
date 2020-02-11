namespace Auto.VehicleCatalog.API.Model
{
    public class Model
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Brand Brand { get; set; }

        public int BrandId { get; set; }    
    }
}