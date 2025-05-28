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

        // Injecting the vehicle service through the constructor
        public VehicleContoller(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        /// <summary>
        /// Retrieves a list of all vehicles in the fleet.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await _vehicleService.GetVehiclesAsync();
            return Ok(vehicles);
        }

        /// <summary>
        /// Retrieves a specific vehicle by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds a new vehicle to the fleet.
        /// </summary>
        /// <param name="createVehicleDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] CreateVehicleDto createVehicleDto)
        {
            return Ok(await _vehicleService.AddVehicleAsync(createVehicleDto));
        }

        /// <summary>
        /// Deletes a vehicle from the fleet by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            return Ok(await _vehicleService.DeleteVehicleAsync(id));
        }


        /// <summary>
        /// Retrieves a list of avaliable vehicles based on the number of passengers and trip length, incuding their profit calculations to help dispachers.
        /// </summary>
        /// <param name="passengers"></param>
        /// <param name="tripLenght"></param>
        /// <returns></returns>
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableVehicles([FromQuery] int passengers, [FromQuery] int tripLenght)
        {
            var result = await _vehicleService.GetVehicleProfitsAsync(passengers, tripLenght);
            return Ok(result);
        }
    }
}
