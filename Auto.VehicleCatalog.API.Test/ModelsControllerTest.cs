using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Auto.VehicleCatalog.API.Controllers;
using Auto.VehicleCatalog.API.Data;
using Auto.VehicleCatalog.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Auto.VehicleCatalog.API.Tests
{
    public class ModelsControllerTest: IClassFixture<DependencyFixture>
    {
        private readonly DependencyFixture _dependencyFixture;

        public ModelsControllerTest(DependencyFixture dependencyFixture)
        {
            _dependencyFixture = dependencyFixture;
        }

        [Theory]
        [InlineData(1)]
        public async void QueryModelList_ReturnsModelsAscendingByName(int brandId)
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.GetModels(brandId))
                .Returns(Task.FromResult<IEnumerable<Model.Model>>(new List<Model.Model>()
                {
                    new Model.Model() {  Id = 1, Name = "Codel 1", BrandId = brandId },
                    new Model.Model() {  Id = 2, Name = "Model 2", BrandId = brandId },
                    new Model.Model() {  Id = 3, Name = "Dodel 3", BrandId = brandId },
                }.OrderBy(o => o.Name)));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<ModelsController>>();
            var controller = new ModelsController(mockRepo.Object, mapper, logger);

            // Act
            var result = await controller.GetModels(brandId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ModelForList>>(okResult.Value);
            var topModel = returnValue.FirstOrDefault().name;
            Assert.Equal("Codel 1", topModel);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        public async void QueryModelIdX_ReturnsCorrectModel(int id)
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.GetModel(id))
                .Returns(Task.FromResult<Model.Model>(new Model.Model() { Id = id, Name = $"Model {id}", BrandId = 1 }));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<ModelsController>>();
            var controller = new ModelsController(mockRepo.Object, mapper, logger);

            // Act
            var result = await controller.GetModel(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ModelForDetail>(okResult.Value);
            var topModel = returnValue.name;
            Assert.Equal($"Model {id}", topModel);
        }

        [Fact]
        public async void AddModel_ReturnModelCreated()
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.SaveAll())
                .Returns( Task.FromResult<bool>(true));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<ModelsController>>();
            var controller = new ModelsController(mockRepo.Object, mapper, logger);
            ModelForNew newModel = new ModelForNew() { name = "Fake", brandId = 1 };

            // Act
            var result = await controller.Post(newModel);

            // Assert
            var okResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Model created.", returnValue);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateModel_ReturnModelUpdated(int id)
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.GetModel(id))
                .Returns( Task.FromResult<Model.Model>(new Model.Model() { Id = id, Name = "Model 1", BrandId = 1 }));            
            mockRepo.Setup(repo => repo.SaveAll())
                .Returns( Task.FromResult<bool>(true));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<ModelsController>>();
            var controller = new ModelsController(mockRepo.Object, mapper, logger);
            ModelForUpdate newModel = new ModelForUpdate() { name = "Fake" };

            // Act
            var result = await controller.Put(id, newModel);

            // Assert
            var okResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Model updated.", returnValue);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteModel_ReturnModelDeleted(int id)
        {
            // Arrange
            var mockRepo = new Mock<IVehicleCatalogRepository>();
            mockRepo.Setup(repo => repo.GetModel(id))
                .Returns( Task.FromResult<Model.Model>(new Model.Model() { Id = id, Name = "Model 1", BrandId = 1 }));            
            mockRepo.Setup(repo => repo.SaveAll())
                .Returns( Task.FromResult<bool>(true));
            var mapper = _dependencyFixture.ServiceProvider.GetService<IMapper>();
            var logger = Mock.Of<ILogger<ModelsController>>();
            var controller = new ModelsController(mockRepo.Object, mapper, logger);

            // Act
            var result = await controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}