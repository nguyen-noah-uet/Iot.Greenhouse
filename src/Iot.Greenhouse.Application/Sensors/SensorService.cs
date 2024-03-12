using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Iot.Greenhouse.Sensors
{
    public class SensorService : CrudAppService<Sensor, SensorDto, int, PagedAndSortedResultRequestDto, SensorCreateDto>, ISensorService
    {
        private readonly IRepository<Sensor, int> _repository;

        public SensorService(IRepository<Sensor, int> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<SensorDto> GetLatestValue(int nodeId, SensorType sensorType, TimeSpan? timeSpan = default)
        {
            if (timeSpan is null)
            {
                timeSpan = TimeSpan.FromHours(1);
            }
            var queryable = await _repository.GetQueryableAsync();
            DateTime since = DateTime.Now - timeSpan.Value;
            var entity = queryable
                .Where(x => x.NodeId == nodeId)
                .Where(x => x.SensorType == sensorType)
                .Where(x => x.CreationTime > since)
                .OrderBy(x => x.Id)
                .LastOrDefault();
            if (entity is null)
            {
                return new SensorDto() { Id = nodeId, SensorType = sensorType, SensorValue = double.NaN};
            }

            return await MapToGetListOutputDtoAsync(entity);

        }

        public async Task<List<SensorDto>> GetLatestValue(int nodeId, SensorType sensorType, int count)
        {
            var queryable = await _repository.GetQueryableAsync();
            List<Sensor> entities = queryable
                .Where(x => x.NodeId == nodeId)
                .Where(x => x.SensorType == sensorType)
                .OrderByDescending(x => x.Id)
                .Take(count)
                .ToList();
            return await MapToGetListOutputDtosAsync(entities);
        }

        public async Task<List<SensorDto>> GetLatestValue(int nodeId, SensorType sensorType, DateTime since)
        {
            var queryable = await _repository.GetQueryableAsync();
            List<Sensor> entities = queryable
                .Where(x => x.NodeId == nodeId)
                .Where(x => x.SensorType == sensorType)
                .Where(x => x.CreationTime > since)
                .OrderByDescending(x => x.Id)
                .ToList();
            return await MapToGetListOutputDtosAsync(entities);
        }
    }
}
