using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiFleetApi.Models;
using TaxiFleetData.Entities;

namespace TaxiFleetBusiness.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Vehicle, VehicleDto>()
                .ForMember(dest => dest.Fuel, opt => opt.MapFrom(src => src.Fuel.ToString()));

            CreateMap<CreateVehicleDto, Vehicle>();
        }
    }
}
