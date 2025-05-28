using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiFleetBusiness.Interfaces;
using TaxiFleetData.Entities;
using TaxiFleetData.Repositories;
using TaxiFleetApi.Models;
using TaxiFleetBusiness.Dtos;

namespace TaxiFleetBusiness.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        public async Task<List<VehicleDto>> GetVehiclesAsync()
        {
            var vehicle = await _vehicleRepository.GetVehiclesAsync();
            return _mapper.Map<List<VehicleDto>>(vehicle);
        }

        public async Task<VehicleDto> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetVehicleByIdAsync(id);
            return _mapper.Map<VehicleDto>(vehicle);
        }
        public async Task<VehicleDto> AddVehicleAsync(CreateVehicleDto createVehicleDto)
        {
            var vehicle = _mapper.Map<Vehicle>(createVehicleDto);
            var addedVehicle = await _vehicleRepository.AddVehicleAsync(vehicle);
            return _mapper.Map<VehicleDto>(addedVehicle);

        }
        public async Task<bool> DeleteVehicleAsync(int id)
        {
            return await _vehicleRepository.DeleteVehicleAsync(id);
        }
        public async Task<List<VehicleProfitDto>> GetVehicleProfitsAsync(int passengers, int tripLenght)
        {
            var allVehicles = await _vehicleRepository.GetVehiclesAsync();
            var result = new List<VehicleProfitDto>();

            foreach (var vehicle in allVehicles)
            {
                var effectiveRange = vehicle.Fuel == FuelType.MildHybrid && tripLenght < 50 ? vehicle.RangeKm * 2 : vehicle.RangeKm;

                if (effectiveRange < tripLenght || passengers > vehicle.PassengerCapacity)
                {
                    continue;
                }
                var minPerKm = tripLenght < 50 ? 2 : 1;
                var totalMin = tripLenght * minPerKm;

                var kmFee = tripLenght * 2m;
                var halfHours = (int)Math.Ceiling(totalMin / 30.0);
                var timeFee = halfHours * 2m;

                var refuelCost = vehicle.Fuel switch
                {
                    FuelType.PureElectric => 1m,
                    _ => 2m
                };

                var profit = (kmFee + timeFee) - refuelCost;

                result.Add(new VehicleProfitDto
                {
                    Id = vehicle.Id,
                    PassengerCapacity = vehicle.PassengerCapacity,
                    RangeKm = vehicle.RangeKm,
                    FuelType = vehicle.Fuel.ToString(),
                    Profit = profit
                });

            }
            return result.OrderByDescending(v => v.Profit).ToList();
        }
    }
}
