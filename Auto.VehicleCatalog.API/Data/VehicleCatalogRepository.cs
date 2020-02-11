using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Auto.VehicleCatalog.API.Model;

namespace Auto.VehicleCatalog.API.Data
{
    public class VehicleCatalogRepository : IVehicleCatalogRepository
    {
        private readonly DataContext _context;

        public VehicleCatalogRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Brand> GetBrand(int id)
        {
            // get brand by id
            var brand = await _context.Brands.FirstOrDefaultAsync(u => u.Id == id);

            return brand;
        }

        public async Task<IEnumerable<Brand>> GetBrands()
        {
            // get all brands ordered by name asc by default
            var brands = await _context.Brands
                .OrderBy(o => o.Name)
                .ToListAsync();

            return brands;
        }

        public async Task<Model.Model> GetModel(int id)
        {
            // get model by id
            var model = await _context.Models
            .Include(b => b.Brand)
            .FirstOrDefaultAsync(u => u.Id == id);

            return model;
        }

        public async Task<IEnumerable<Model.Model>> GetModels(int brandId)
        {
             // get all models filtered by brand id and ordered by name asc by default
            var models = await _context.Models
                .Include(b => b.Brand)
                .Where(w => w.BrandId == brandId)
                .OrderBy(o => o.Name)
                .ToListAsync();

            return models;
        }

        public async Task<Vehicle> GetVehicle(int id)
        {
            // get vehicle by id
            var vehicle = await _context.Vehicles
            .Include(b => b.Brand)
            .Include(m => m.Model)
            .FirstOrDefaultAsync(u => u.Id == id);

            return vehicle;
        }

        public async Task<IEnumerable<Vehicle>> GetVehicles(int modelId)
        {
             // get all vehicles filtered by model id and ordered by value asc by default
            var vehicles = await _context.Vehicles
                .Include(b => b.Brand)
                .Include(m => m.Model)
                .Where(w => w.ModelId == modelId)
                .OrderBy(o => o.Value)
                .ToListAsync();

            return vehicles;
        }
    }
}
