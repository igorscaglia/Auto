using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Auto.VehicleCatalog.API.Data;
using Auto.VehicleCatalog.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Auto.VehicleCatalog.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ModelsController : ControllerBase
    {
        private readonly IVehicleCatalogRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ModelsController> _logger;

        public ModelsController(IVehicleCatalogRepository repository,
            IMapper mapper,
            ILogger<ModelsController> logger)
        {
            // All passed by injection
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpGet("brand/{brandId}", Name = nameof(GetModels))]
        public async Task<IActionResult> GetModels(int brandId)
        {
            // get all models by brand id
            var models = await _repository.GetModels(brandId);

            // map to dto
            var modelsResult = _mapper.Map<IEnumerable<ModelForList>>(models);

            // return response
            return Ok(modelsResult);
        }

        [HttpGet("{id}", Name = nameof(GetModel))]
        public async Task<IActionResult> GetModel(int id)
        {
            // get model by id
            var model = await _repository.GetModel(id);

            // map to dto
            var modelResult = _mapper.Map<ModelForDetail>(model);

            // return response
            return Ok(modelResult);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ModelForNew model)
        {
            // map to dto
            var modelResult = _mapper.Map<Model.Model>(model);

            // Add new model
            _repository.Add<Model.Model>(modelResult);

            // Save it on database
            if (await _repository.SaveAll())
            {
                return CreatedAtRoute(nameof(GetModel), new { id = modelResult.Id }, "Model created.");
            }
            else
            {
                string errorMsg = "Failed adding model on server.";
                _logger.LogError(errorMsg);
                throw new Exception(errorMsg);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, ModelForUpdate modelForUpdate)
        {
            var model = await _repository.GetModel(id);

            // If it's null then there isn't a model with the param id
            if (model == null)
            {
                return NotFound(new { Message = $"Model with id {id} not found." });
            }
            else
            {
                model.Name = modelForUpdate.name;

                // Update brand id only if it has been passed
                if (modelForUpdate.brandId.HasValue)
                    model.BrandId = modelForUpdate.brandId.Value;

                // Save it on database
                if (await _repository.SaveAll())
                {
                    return CreatedAtRoute(nameof(GetModel), new { id = model.Id }, "Model updated.");
                }
                else
                {
                    string errorMsg = "Failed updating model on server.";
                    _logger.LogError(errorMsg);
                    throw new Exception(errorMsg);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _repository.GetModel(id);

            // If it's null then there isn't a model with the param id
            if (model == null)
            {
                return NotFound(new { Message = $"Model with id {id} not found." });
            }
            else
            {
                _repository.Delete<Model.Model>(model);

                // Save it on database
                if (await _repository.SaveAll())
                {
                    return NoContent();
                }
                else
                {
                    string errorMsg = "Failed deleting model on server.";
                    _logger.LogError(errorMsg);
                    throw new Exception(errorMsg);
                }
            }
        }
    }
}
