using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiFleetData.Entities;
using TaxiFleetData.Migrations;

namespace TaxiFleetData.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly TaxiFleetDbContext _context;

        // Constructor with dependency injection for the DbContext
        public VehicleRepository(TaxiFleetDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new vehicle to the fleet.
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        /// <summary>
        /// Gets a vehicle by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        /// <summary>
        /// Gets a list of all vehicles in the fleet.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        /// <summary>
        /// Deletes a vehicle from the fleet by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteVehicleAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return false;
            }
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
