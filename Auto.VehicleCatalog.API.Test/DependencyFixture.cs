using AutoMapper;
using Auto.VehicleCatalog.API.Data;
using Auto.VehicleCatalog.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auto.VehicleCatalog.API.Tests
{
   public class DependencyFixture
    {
        public DependencyFixture()
        {
            var serviceCollection = new ServiceCollection();

            // Add in memory ef database
            serviceCollection.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName: "VehicleCatalog"));
            
            //Auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            serviceCollection.AddSingleton<IMapper>(mapper);

            // Build de DI
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }
}
