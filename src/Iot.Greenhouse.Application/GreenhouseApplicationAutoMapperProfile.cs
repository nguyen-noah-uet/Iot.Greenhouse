using AutoMapper;
using Iot.Greenhouse.Nodes;
using Iot.Greenhouse.Sensors;

namespace Iot.Greenhouse;

public class GreenhouseApplicationAutoMapperProfile : Profile
{
    public GreenhouseApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Node, NodeDto>();
        CreateMap<NodeCreateUpdateDto, Node>();
        CreateMap<Sensor, SensorDto>();
        CreateMap<SensorCreateDto, Sensor>();
    }
}
