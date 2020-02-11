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
    public class BrandsController : ControllerBase
    {
        private readonly IVehicleCatalogRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<BrandsController> _logger;

        public BrandsController(IVehicleCatalogRepository repository,
            IMapper mapper,
            ILogger<BrandsController> logger)
        {
            // All passed by injection
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBrands()
        {
            // get all brands 
            var brands = await _repository.GetBrands();

            // map to dto
            var brandsResult = _mapper.Map<IEnumerable<BrandForList>>(brands);

            // return response
            return Ok(brandsResult);
        }

        [HttpGet("{id}", Name = nameof(GetBrand))]
        public async Task<IActionResult> GetBrand(int id)
        {
            // get brand by id
            var brand = await _repository.GetBrand(id);

            // map to dto
            var brandResult = _mapper.Map<BrandForDetail>(brand);

            // return response
            return Ok(brandResult);
        }

        [HttpPost]
        public async Task<IActionResult> Post(BrandForNew brand)
        {
            // map to dto
            var brandResult = _mapper.Map<Brand>(brand);

            // Add new brand
            _repository.Add<Brand>(brandResult);

            // Save it on database
            if (await _repository.SaveAll())
            {
                return CreatedAtRoute(nameof(GetBrand), new { id = brandResult.Id }, "Brand created.");
            }
            else
            {
                string errorMsg = "Failed adding brand on server.";
                _logger.LogError(errorMsg);
                throw new Exception(errorMsg);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, BrandForUpdate brandForUpdate)
        {
            var brand = await _repository.GetBrand(id);

            // If it's null then there isn't a brand with the param id
            if (brand == null)
            {
                return NotFound(new { Message = $"Brand with id {id} not found." });
            }
            else
            {
                brand.Name = brandForUpdate.name;

                // Save it on database
                if (await _repository.SaveAll())
                {
                    return CreatedAtRoute(nameof(GetBrand), new { id = brand.Id }, "Brand updated.");
                }
                else
                {
                    string errorMsg = "Failed updating brand on server";
                    _logger.LogError(errorMsg);
                    throw new Exception(errorMsg);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _repository.GetBrand(id);

            // If it's null then there isn't a brand with the param id
            if (brand == null)
            {
                return NotFound(new { Message = $"Brand with id {id} not found." });
            }
            else
            {
                _repository.Delete<Brand>(brand);

                // Save it on database
                if (await _repository.SaveAll())
                {
                    return NoContent();
                }
                else
                {
                    string errorMsg = "Failed deleting brand on server";
                    _logger.LogError(errorMsg);
                    throw new Exception(errorMsg);
                }
            }
        }
    }
}
