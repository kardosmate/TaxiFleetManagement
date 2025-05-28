using Microsoft.AspNetCore.Mvc;
using TaxiFleetApi.Models;
using TaxiFleetBusiness.Interfaces;
using TaxiFleetData.Entities;

namespace TaxiFleetManagement.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VehicleContoller : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehicleContoller(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await _vehicleService.GetVehiclesAsync();
            return Ok(vehicles);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound($"Vehicle by ID {id} not found.");
            }
            return Ok(vehicle);
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] CreateVehicleDto createVehicleDto)
        {
            return Ok(await _vehicleService.AddVehicleAsync(createVehicleDto));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            return Ok(await _vehicleService.DeleteVehicleAsync(id));
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableVehicles([FromQuery] int passengers, [FromQuery] int tripLenght)
        {
            var result = await _vehicleService.GetVehicleProfitsAsync(passengers, tripLenght);
            return Ok(result);
        }
    }
}
