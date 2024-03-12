using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iot.Greenhouse.Nodes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Iot.Greenhouse.Sensors
{
    public interface ISensorService : ICrudAppService<SensorDto, int, PagedAndSortedResultRequestDto, SensorCreateDto>
    {
        Task<SensorDto> GetLatestValue(int nodeId, SensorType sensorType, TimeSpan? timeSpan = default);
        Task<List<SensorDto>> GetLatestValue(int nodeId, SensorType sensorType, int count);
        Task<List<SensorDto>> GetLatestValue(int nodeId, SensorType sensorType, DateTime since);
    }

    public class SensorDto : CreationAuditedEntityDto<int>
    {
        public SensorType SensorType { get; set; }
        public double SensorValue { get; set; }
        public NodeDto Node { get; set; } = default!;
    }

    public class SensorCreateDto
    {
        public SensorType SensorType { get; set; }
        public double SensorValue { get; set; }
        public int NodeId { get; set; }
    }
}
