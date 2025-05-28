using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiFleetData.Entities
{
    /// <summary>
    /// Represents a vehicle in the taxi fleet.
    /// </summary>
    public class Vehicle
    {
        public int Id { get; set; }
        public int PassengerCapacity { get; set; }
        public int RangeKm { get; set; }
        public FuelType Fuel { get; set; }
    }

    /// <summary>
    /// Available fuel types for vehicles.
    /// </summary>
    public enum FuelType
    {
        Gasoline,
        MildHybrid,
        PureElectric
    }
}
