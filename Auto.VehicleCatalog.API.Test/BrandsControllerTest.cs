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
    public class BrandsControllerTest : IClassFixture<DependencyFixture>
    {
        private readonly DependencyFixture _dependencyFixture;

        public BrandsControllerTest(DependencyFixture dependencyFixture)
        {
            _dependencyFixture = dependencyFixture;
        }

        [Fact]
        public async void QueryBrandList_ReturnsBrandsAscendingByName()
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.GetBrands())
                .Returns(Task.FromResult<IEnumerable<Brand>>(new List<Brand>()
                {
                    new Brand() {  Id = 1, Name = "Crand 1" },
                    new Brand() {  Id = 2, Name = "Brand 2" },
                    new Brand() {  Id = 3, Name = "Drand 3" },
                }.OrderBy(o => o.Name)));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<BrandsController>>();
            var controller = new BrandsController(mockRepo.Object, mapper, logger);

            // Act
            var result = await controller.GetBrands();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<BrandForList>>(okResult.Value);
            var topBrand = returnValue.FirstOrDefault().name;
            Assert.Equal("Brand 2", topBrand);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        public async void QueryBrandIdX_ReturnsCorrectBrand(int id)
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.GetBrand(id))
                .Returns(Task.FromResult<Brand>(new Brand() { Id = id, Name = $"Brand {id}" }));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<BrandsController>>();
            var controller = new BrandsController(mockRepo.Object, mapper, logger);

            // Act
            var result = await controller.GetBrand(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BrandForDetail>(okResult.Value);
            var topBrand = returnValue.name;
            Assert.Equal($"Brand {id}", topBrand);
        }

        [Fact]
        public async void AddBrand_ReturnBrandCreated()
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.SaveAll())
                .Returns( Task.FromResult<bool>(true));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<BrandsController>>();
            var controller = new BrandsController(mockRepo.Object, mapper, logger);
            BrandForNew newBrand = new BrandForNew() { name = "Fake" };

            // Act
            var result = await controller.Post(newBrand);

            // Assert
            var okResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Brand created.", returnValue);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateBrand_ReturnBrandUpdated(int id)
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.GetBrand(id))
                .Returns( Task.FromResult<Brand>(new Brand() { Id = id, Name = "Brand 1" }));            
            mockRepo.Setup(repo => repo.SaveAll())
                .Returns( Task.FromResult<bool>(true));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<BrandsController>>();
            var controller = new BrandsController(mockRepo.Object, mapper, logger);
            BrandForUpdate newBrand = new BrandForUpdate() { name = "Fake" };

            // Act
            var result = await controller.Put(id, newBrand);

            // Assert
            var okResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Brand updated.", returnValue);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteBrand_ReturnBrandDeleted(int id)
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.GetBrand(id))
                .Returns( Task.FromResult<Brand>(new Brand() { Id = id, Name = "Brand 1" }));            
            mockRepo.Setup(repo => repo.SaveAll())
                .Returns( Task.FromResult<bool>(true));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<BrandsController>>();
            var controller = new BrandsController(mockRepo.Object, mapper, logger);

            // Act
            var result = await controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}