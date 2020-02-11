using System.Collections.Generic;

namespace Auto.VehicleCatalog.API.Model
{
    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Model> Models { get; set; }
    }
}