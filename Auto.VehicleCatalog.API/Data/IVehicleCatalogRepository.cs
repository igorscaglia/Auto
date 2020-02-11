using System.Collections.Generic;
using System.Threading.Tasks;
using Auto.VehicleCatalog.API.Model;

namespace Auto.VehicleCatalog.API.Data
{
    public interface IVehicleCatalogRepository
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();

        Task<IEnumerable<Brand>> GetBrands();

        Task<Brand> GetBrand(int id);

        Task<IEnumerable<Model.Model>> GetModels(int brandId);

        Task<Model.Model> GetModel(int id);

        Task<IEnumerable<Vehicle>> GetVehicles(int modelId);

        Task<Vehicle> GetVehicle(int id);
    }
}
