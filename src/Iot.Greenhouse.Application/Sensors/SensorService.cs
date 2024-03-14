using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Iot.Greenhouse.Sensors
{
    public class SensorService : CrudAppService<Sensor, SensorDto, Guid, PagedAndSortedResultRequestDto, SensorCreateDto>, ISensorService
    {
        private readonly IRepository<Sensor, Guid> _repository;

        public SensorService(IRepository<Sensor, Guid> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<List<SensorDto>> GetByNodeId(Guid nodeId)
        {
            var queryable = await _repository.GetQueryableAsync();
            var entities = queryable.Where(s => s.NodeId.CompareTo(nodeId) == 0) .ToList();
            return await MapToGetListOutputDtosAsync(entities);
        }
    }
}
