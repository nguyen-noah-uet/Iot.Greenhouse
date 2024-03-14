using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Iot.Greenhouse.Sensors
{
    public class SensorDataService : CrudAppService<SensorData, SensorDataDto, int, PagedAndSortedResultRequestDto, SensorDataCreateDto>, ISensorDataService
    {
        private readonly IRepository<SensorData, int> repository;

        public SensorDataService(IRepository<SensorData, int> repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<SensorDataDto> GetLatestData(Guid sensorId, TimeSpan? timeSpan = null)
        {
            if (timeSpan is null)
            {
                timeSpan = TimeSpan.FromHours(2);
            }
            var queryable = await repository.GetQueryableAsync();

            DateTime since = DateTime.Now - timeSpan.Value;
            var entity = queryable
                .Where(x => x.SensorId == sensorId && x.CreationTime > since)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
            if (entity is null)
            {
                return new SensorDataDto()
                {
                    SensorId = sensorId,
                    Value = double.NaN,
                };
            }
            return MapToGetOutputDto(entity);

        }

        public async Task<List<SensorDataDto>> GetLatestDataList(Guid sensorId, TimeSpan? timeSpan = null)
        {
            if(timeSpan is null)
            {
                timeSpan = TimeSpan.FromHours(24);
            }
            var queryable = await repository.GetQueryableAsync();
            DateTime since = DateTime.Now - timeSpan.Value;
            var entities = queryable
                .Where(x => x.SensorId == sensorId && x.CreationTime > since)
                .OrderByDescending(x => x.Id)
                .ToList();
            return await MapToGetListOutputDtosAsync(entities);
        }

        public async Task<List<SensorDataDto>> GetLatestDataListByCount(Guid sensorId, int count = 100)
        {
            var queryable = await repository.GetQueryableAsync();
            var entities = queryable
                .Where(x => x.SensorId == sensorId)
                .OrderByDescending(x => x.Id)
                .Take(count)
                .ToList();
            return await MapToGetListOutputDtosAsync(entities);
        }
    }
}
