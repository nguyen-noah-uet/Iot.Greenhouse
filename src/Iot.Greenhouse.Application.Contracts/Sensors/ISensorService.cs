using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iot.Greenhouse.Nodes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Iot.Greenhouse.Sensors
{
    public interface ISensorService : ICrudAppService<SensorDto, Guid, PagedAndSortedResultRequestDto, SensorCreateDto>
    {
        Task<List<SensorDto>> GetByNodeId(Guid nodeId);
    }

    public class SensorDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid NodeId { get; set; }
        public SensorType SensorType { get; set; }
        public string? Unit { get; set; }
    }

    public class SensorCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid NodeId { get; set; }
        public SensorType SensorType { get; set; }
        public string? Unit { get; set; }
    }
}
