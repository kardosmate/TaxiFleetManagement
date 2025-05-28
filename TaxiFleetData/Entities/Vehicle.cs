using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiFleetData.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int PassengerCapacity { get; set; }
        public int RangeKm { get; set; }
        public FuelType Fuel { get; set; }
    }

    public enum FuelType
    {
        Gasoline,
        MildHybrid,
        PureElectric
    }
}
