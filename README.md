# Taxi Fleet Management API Documentation

## Table of Contents
1. [Overview](#overview)
2. [Technology Stack](#technology-stack)
3. [API Endpoints](#api-endpoints)
4. [Data Models](#data-models)
5. [Business Logic](#business-logic)
6. [Testing](#testing)

## 1. Overview <a name="overview"></a>
The Taxi Fleet Management API is a backend system for managing a fleet of taxis and calculating trip profitability. The system handles:
- Vehicle inventory management
- Profit calculation based on trip parameters
- Availability filtering based on vehicle capabilities

## 2. Technology Stack <a name="technology-stack"></a>
- **Backend**: ASP.NET Core 8
- **Database**: SQL Server with Entity Framework Core
- **API Documentation**: Swagger/OpenAPI
- **Architecture**: Clean Architecture (API-Business-Data layers)
- **Testing**: Swagger UI

## 3. API Endpoints <a name="api-endpoints"></a>

### Vehicle Management
| Endpoint | Method | Description | Parameters | Example |
|----------|--------|-------------|------------|---------|
| `/api/Vehicle` | GET | Get all vehicles | None | [GET] /api/Vehicle |
| `/api/Vehicle/{id}` | GET | Get vehicle by ID | `id`: Vehicle ID | [GET] /api/Vehicle/5 |
| `/api/Vehicle` | POST | Create new vehicle | JSON: CreateVehicleDto | [POST] /api/Vehicle |
| `/api/Vehicle/{id}` | DELETE | Delete vehicle | `id`: Vehicle ID | [DELETE] /api/Vehicle/5 |

### Trip Planning
| Endpoint | Method | Description | Parameters | Example |
|----------|--------|-------------|------------|---------|
| `/api/Vehicle/available` | GET | Get available vehicles with profit | `passengers`: Number of passengers<br>`tripLength`: Trip distance in km | [GET] /api/Vehicle/available?passengers=4&tripLength=40 |

## 4. Data Models <a name="data-models"></a>

### Vehicle Entity
```csharp
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
```

### DTOs
**CreateVehicleDto (POST Request)**
```json
{
  "passengerCapacity": 4,
  "rangeKm": 500,
  "fuel": "Gasoline"
}
```

**VehicleDto (Response)**
```json
{
  "id": 1,
  "passengerCapacity": 4,
  "rangeKm": 500,
  "fuel": "Gasoline"
}
```

**VehicleProfitDto (Trip Planning Response)**
```json
{
  "id": 1,
  "passengerCapacity": 4,
  "rangeKm": 300,
  "fuelType": "MildHybrid",
  "profit": 84.0
}
```

## 5. Business Logic <a name="business-logic"></a>

### Vehicle Availability Rules
A vehicle is available if:
1. Its effective range ≥ trip length
   - For MildHybrid vehicles: trips <50km, effective range = 2 * RangeKm (this is not accurate however its only for incuding them in the list of available vehicles), trips ≥ 50 km, effective range = RangeKm + 25 (the first  50km only counts for 25km so ve add it).
   - All other cases: effective range = RangeKm
2. PassengerCapacity ≥ requested passengers

### Profit Calculation
```math
Profit = (DistanceFee + TimeFee) - RefuelCost
```

**Components:**
1. **Distance Fee**: 2€ per km * passengers
   - `DistanceFee = tripLength * 2`
   
2. **Time Fee**: 2€ per started half-hour
   - Time calculation:
     - Trips <50km: 2 minutes per km
     - Trips ≥50km: 1 minute per km + 50 minutes (for the first 50km)
   - `TimeFee = ⌈totalMinutes / 30⌉ * 2 * passengers`

3. **Refuel Cost**:
   - Gasoline: 2€ per km
   - MildHybrid: 2€ per km
   - PureElectric: 1€ per km

### Calculation Example
**City Trip (40km) with MildHybrid Vehicle 3 passengers:**
```
DistanceFee = 40 * 2 * 3 = 240€
Time = 40 * 2 = 80 minutes -> 80/30 = 2.66 -> 3 half-hours
TimeFee = 3 * 2 * 3 = 18€
RefuelCost = 2 * 40 = 80€
Profit = (240 + 18) - 80 = 178€
```

**Long Trip (100km) with PureElectric Vehicle 2 passengers:**
```
DistanceFee = 100 * 2 * 2 = 400€
Time = 100 * 1 + 50 = 150 minutes -> 150/30 = 5 -> 5 half-hours
TimeFee = 5 * 2 * 2 = 20€
RefuelCost = 100 * 1€
Profit = (400 + 20) - 100 = 320€
```

## 6. Testing <a name="testing"></a>

### Using Swagger
1. Run the application
2. Navigate to `https://localhost:7058/swagger` or `http://localhost:5242/swagger`
3. Test endpoints directly in the browser


Sample Requests:
```json
// POST /api/VehicleController
{
  "passengerCapacity": 4,
  "rangeKm": 500,
  "fuel": "Gasoline"
}

// GET /api/VehicleController/available?passengers=4&tripLength=40
```
