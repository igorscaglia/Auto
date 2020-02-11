using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Auto.VehicleCatalog.API.Controllers;
using Auto.VehicleCatalog.API.Data;
using Auto.VehicleCatalog.API.Dtos;
using Auto.VehicleCatalog.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Auto.VehicleCatalog.API.Tests
{
    public class VehiclesControllerTest : IClassFixture<DependencyFixture>
    {
        private readonly DependencyFixture _dependencyFixture;

        public VehiclesControllerTest(DependencyFixture dependencyFixture)
        {
            _dependencyFixture = dependencyFixture;
        }

        [Theory]
        [InlineData(1)]
        public async void QueryVehicleList_ReturnsVehiclesAscendingByValue(int modelId)
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.GetVehicles(modelId))
                .Returns(Task.FromResult<IEnumerable<Vehicle>>(new List<Vehicle>()
                {
                    new Vehicle() {  Id = 1, Value = 15488.45M, BrandId = 1, ModelId = modelId, YearModel = 1998, Fuel = "Gasoline" },
                    new Vehicle() {  Id = 2, Value = 18484.54M, BrandId = 1, ModelId = modelId, YearModel = 1996, Fuel = "Gasoline" },
                    new Vehicle() {  Id = 3, Value = 10000.00M, BrandId = 1, ModelId = modelId, YearModel = 1997, Fuel = "Gasoline" },
                }.OrderBy(o => o.Value)));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<VehiclesController>>();
            var controller = new VehiclesController(mockRepo.Object, mapper, logger);

            // Act
            var result = await controller.GetVehicles(modelId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<VehicleForList>>(okResult.Value);
            var topVehicle = returnValue.FirstOrDefault().value;
            Assert.Equal("R$ 10.000,00", topVehicle);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        public async void QueryVehicleIdX_ReturnsCorrectVehicle(int id)
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.GetVehicle(id))
                .Returns(Task.FromResult<Vehicle>(new Vehicle() { Id = id, Value = 10000.00M, BrandId = 1, ModelId = 1, YearModel = 1998, Fuel = "Gasoline" }));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<VehiclesController>>();
            var controller = new VehiclesController(mockRepo.Object, mapper, logger);

            // Act
            var result = await controller.GetVehicle(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<VehicleForDetail>(okResult.Value);
            var topVehicle = returnValue.value;
            Assert.Equal("R$ 10.000,00", topVehicle);
        }

        [Fact]
        public async void AddVehicle_ReturnVehicleCreated()
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.SaveAll())
                .Returns(Task.FromResult<bool>(true));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<VehiclesController>>();
            var controller = new VehiclesController(mockRepo.Object, mapper, logger);
            VehicleForNew newVehicle = new VehicleForNew()
            {
                value = 18000.50M,
                brandId = 1,
                modelId = 1,
                yearModel = 1998,
                fuel = "Gasoline"
            };

            // Act
            var result = await controller.Post(newVehicle);

            // Assert
            var okResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Vehicle created.", returnValue);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateVehicle_ReturnVehicleUpdated(int id)
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.GetVehicle(id))
                .Returns(Task.FromResult<Vehicle>(new Vehicle() { Id = id, Value = 10000.00M, BrandId = 1, ModelId = 1, YearModel = 1998, Fuel = "Gasoline" }));
            mockRepo.Setup(repo => repo.SaveAll())
                .Returns(Task.FromResult<bool>(true));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<VehiclesController>>();
            var controller = new VehiclesController(mockRepo.Object, mapper, logger);
            VehicleForUpdate newVehicle = new VehicleForUpdate() 
            {
                value = 18000.50M,
                brandId = 1,
                modelId = 1,
                yearModel = 1998,
                fuel = "Gasoline"
            };

            // Act
            var result = await controller.Put(id, newVehicle);

            // Assert
            var okResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Vehicle updated.", returnValue);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteVehicle_ReturnVehicleDeleted(int id)
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.GetVehicle(id))
                .Returns(Task.FromResult<Vehicle>(new Vehicle() { Id = id, Value = 10000.00M, BrandId = 1, ModelId = 1, YearModel = 1998, Fuel = "Gasoline" }));
            mockRepo.Setup(repo => repo.SaveAll())
                .Returns(Task.FromResult<bool>(true));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<VehiclesController>>();
            var controller = new VehiclesController(mockRepo.Object, mapper, logger);

            // Act
            var result = await controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
