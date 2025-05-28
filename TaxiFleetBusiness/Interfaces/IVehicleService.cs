using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiFleetData.Entities;
using TaxiFleetApi.Models;
using TaxiFleetBusiness.Dtos;

namespace TaxiFleetBusiness.Interfaces
{
    public interface IVehicleService
    {
        Task<List<VehicleDto>> GetVehiclesAsync();
        Task<VehicleDto> GetVehicleByIdAsync(int id);
        Task<VehicleDto> AddVehicleAsync(CreateVehicleDto createVehicleDto);
        Task<bool> DeleteVehicleAsync(int id);

        Task<List<VehicleProfitDto>> GetVehicleProfitsAsync(int passengers, int tripLenght);
    }
}
