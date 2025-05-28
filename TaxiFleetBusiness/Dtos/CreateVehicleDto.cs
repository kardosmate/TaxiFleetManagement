using System.ComponentModel.DataAnnotations;
using TaxiFleetData.Entities;

namespace TaxiFleetApi.Models
{
    /// <summary>
    /// Data Transfer Object for creating a new vehicle
    /// </summary>
    public class CreateVehicleDto
    {
        [Required]
        [Range(1,10)]
        public int PassengerCapacity { get; set; }

        [Required]
        [Range(1, 1500)]
        public int RangeKm { get; set; }

        [Required]
        [EnumDataType(typeof(FuelType))]
        public FuelType Fuel { get; set; }
    }
}
