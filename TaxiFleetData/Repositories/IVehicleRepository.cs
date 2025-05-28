using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiFleetData.Entities;

namespace TaxiFleetData.Repositories
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetVehiclesAsync();
        Task<Vehicle> GetVehicleByIdAsync(int id);
        Task<Vehicle> AddVehicleAsync(Vehicle vehicle);
        Task<bool> DeleteVehicleAsync(int id);


    }
}
