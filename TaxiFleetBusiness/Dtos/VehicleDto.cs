namespace TaxiFleetApi.Models
{
    /// <summary>
    /// Data Transfer Object for Vehicle
    /// </summary>
    public class VehicleDto
    {
        public int Id { get; set; }
        public int PassengerCapacity { get; set; }
        public int RangeKm { get; set; }
        public string Fuel { get; set; }
    }
}
