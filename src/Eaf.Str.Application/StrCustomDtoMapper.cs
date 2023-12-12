using AutoMapper;
using Eaf.Str.Airplanes;
using Eaf.Str.Airplanes.Dtos;

namespace Eaf.Str
{
    internal static class StrCustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */

            configuration.CreateMap<CreateOrEditAirplaneDto, Airplane>();
            configuration.CreateMap<Airplane, AirplaneDto>();
        }
    }
}