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

        // Constructor with dependency injection for repository and mapper
        public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of all vehicles.
        /// </summary>
        /// <returns></returns>
        public async Task<List<VehicleDto>> GetVehiclesAsync()
        {
            var vehicle = await _vehicleRepository.GetVehiclesAsync();
            return _mapper.Map<List<VehicleDto>>(vehicle);
        }

        /// <summary>
        /// Gets a vehicle by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<VehicleDto> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetVehicleByIdAsync(id);
            return _mapper.Map<VehicleDto>(vehicle);
        }

        /// <summary>
        /// Adds a new vehicle to the fleet.
        /// </summary>
        /// <param name="createVehicleDto"></param>
        /// <returns></returns>
        public async Task<VehicleDto> AddVehicleAsync(CreateVehicleDto createVehicleDto)
        {
            var vehicle = _mapper.Map<Vehicle>(createVehicleDto);
            var addedVehicle = await _vehicleRepository.AddVehicleAsync(vehicle);
            return _mapper.Map<VehicleDto>(addedVehicle);

        }

        /// <summary>
        /// Deletes a vehicle by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteVehicleAsync(int id)
        {
            return await _vehicleRepository.DeleteVehicleAsync(id);
        }

        /// <summary>
        /// Gets a list of vehicles with their profits based on the number of passengers and trip length.
        /// </summary>
        /// <param name="passengers"></param>
        /// <param name="tripLenght"></param>
        /// <returns></returns>
        public async Task<List<VehicleProfitDto>> GetVehicleProfitsAsync(int passengers, int tripLenght)
        {
            var allVehicles = await _vehicleRepository.GetVehiclesAsync();
            var result = new List<VehicleProfitDto>();

            foreach (var vehicle in allVehicles)
            {
                // For hybrid vehicles, we double the range for short trips
                var effectiveRange = vehicle.Fuel == FuelType.MildHybrid && tripLenght < 50 ? vehicle.RangeKm * 2 : vehicle.RangeKm + 25;

                // Check if the vehicle can handle the trip
                if (effectiveRange < tripLenght || passengers > vehicle.PassengerCapacity)
                {
                    continue;
                }

                // Calculate the time in minutes based on trip length
                var totalMin = tripLenght < 50 ? tripLenght * 2 : tripLenght + 50;

                // Calculate fees based on trip length and time
                var kmFee = tripLenght * 2m * passengers;
                var halfHours = (int)Math.Ceiling(totalMin / 30.0);
                var timeFee = halfHours * 2m * passengers;

                // Calculate refuel cost based on fuel type
                var refuelCost = vehicle.Fuel switch
                {
                    FuelType.PureElectric => tripLenght * 1m,
                    _ => tripLenght * 2m
                };

                // Calculate profit
                var profit = (kmFee + timeFee) - refuelCost;

                // Add the vehicle and its profit to the result list using DTO
                result.Add(new VehicleProfitDto
                {
                    Id = vehicle.Id,
                    PassengerCapacity = vehicle.PassengerCapacity,
                    RangeKm = vehicle.RangeKm,
                    FuelType = vehicle.Fuel.ToString(),
                    Profit = profit
                });

            }
            // Order the result by profit in descending order to help dispatchers choose the most profitable vehicle
            return result.OrderByDescending(v => v.Profit).ToList();
        }
    }
}
