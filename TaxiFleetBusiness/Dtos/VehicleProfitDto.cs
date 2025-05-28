using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiFleetBusiness.Dtos
{
    /// <summary>
    /// Data Transfer Object for vehicle profit information.
    /// </summary>
    public class VehicleProfitDto
    {
        public int Id { get; set; }
        public int PassengerCapacity { get; set; }
        public int RangeKm { get; set; }
        public string FuelType { get; set; }
        public decimal Profit { get; set; }
    }
}
