using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Auto.VehicleCatalog.API.Data;
using Auto.VehicleCatalog.API.Dtos;
using Auto.VehicleCatalog.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Auto.VehicleCatalog.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleCatalogRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IVehicleCatalogRepository repository,
            IMapper mapper,
            ILogger<VehiclesController> logger)
        {
            // All passed by injection
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpGet("model/{modelId}", Name = nameof(GetVehicles))]
        public async Task<IActionResult> GetVehicles(int modelId)
        {
            // get all Vehicles by brand id
            var vehicles = await _repository.GetVehicles(modelId);

            // map to dto
            var vehiclesResult = _mapper.Map<IEnumerable<VehicleForList>>(vehicles);

            // return response
            return Ok(vehiclesResult);
        }

        [HttpGet("{id}", Name = nameof(GetVehicle))]
        public async Task<IActionResult> GetVehicle(int id)
        {
            // get vehicle by id
            var vehicle = await _repository.GetVehicle(id);

            // map to dto
            var vehicleResult = _mapper.Map<VehicleForDetail>(vehicle);

            // return response
            return Ok(vehicleResult);
        }

        [HttpPost]
        public async Task<IActionResult> Post(VehicleForNew vehicle)
        {
            // map to dto
            var vehicleResult = _mapper.Map<Vehicle>(vehicle);

            // Add new vehicle
            _repository.Add<Vehicle>(vehicleResult);

            // Save it on database
            if (await _repository.SaveAll())
            {
                return CreatedAtRoute(nameof(GetVehicle), new { id = vehicleResult.Id }, "Vehicle created.");
            }
            else
            {
                string errorMsg = "Failed adding vehicle on server.";
                _logger.LogError(errorMsg);
                throw new Exception(errorMsg);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, VehicleForUpdate VehicleForUpdate)
        {
            var Vehicle = await _repository.GetVehicle(id);

            // If it's null then there isn't a vehicle with the param id
            if (Vehicle == null)
            {
                return NotFound(new { Message = $"Vehicle with id {id} not found." });
            }
            else
            {
                Vehicle.Value = VehicleForUpdate.value;
                Vehicle.YearModel = VehicleForUpdate.yearModel;
                Vehicle.Fuel = VehicleForUpdate.fuel;

                // Update brand id only if it has been passed
                if (VehicleForUpdate.brandId.HasValue)
                    Vehicle.BrandId = VehicleForUpdate.brandId.Value;

                // Update model id only if it has been passed
                if (VehicleForUpdate.modelId.HasValue)
                    Vehicle.ModelId = VehicleForUpdate.modelId.Value;

                // Save it on database
                if (await _repository.SaveAll())
                {
                    return CreatedAtRoute(nameof(GetVehicle), new { id = Vehicle.Id }, "Vehicle updated.");
                }
                else
                {
                    string errorMsg = "Failed updating vehicle on server.";
                    _logger.LogError(errorMsg);
                    throw new Exception(errorMsg);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _repository.GetVehicle(id);

            // If it's null then there isn't a Vehicle with the param id
            if (vehicle == null)
            {
                return NotFound(new { Message = $"Vehicle with id {id} not found." });
            }
            else
            {
                _repository.Delete<Vehicle>(vehicle);

                // Save it on database
                if (await _repository.SaveAll())
                {
                    return NoContent();
                }
                else
                {
                    string errorMsg = "Failed deleting vehicle on server.";
                    _logger.LogError(errorMsg);
                    throw new Exception(errorMsg);
                }
            }
        }
    }
}
