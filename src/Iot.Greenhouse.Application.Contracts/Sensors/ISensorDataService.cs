using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Iot.Greenhouse.Sensors
{
    public interface ISensorDataService : ICrudAppService<SensorDataDto, int, PagedAndSortedResultRequestDto, SensorDataCreateDto>
    {
        Task<SensorDataDto> GetLatestData(Guid sensorId, TimeSpan? timeSpan = default);
        Task<List<SensorDataDto>> GetLatestDataList(Guid sensorId, TimeSpan? timeSpan = default);
        Task<List<SensorDataDto>> GetLatestDataListByCount(Guid sensorId, int count = 100);
    }

    public class SensorDataDto : CreationAuditedEntityDto<int>
    {
        public Guid SensorId { get; set; }
        public double Value { get; set; }
        public SensorDto Sensor { get; set; } = default!;
    }

    public class SensorDataCreateDto
    {
        public Guid SensorId { get; set;}
        public double Value { get; set; }
    }
}
