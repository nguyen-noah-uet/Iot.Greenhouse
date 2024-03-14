using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Iot.Greenhouse.Devices
{
    public class DeviceService : CrudAppService<Device, DeviceDto, Guid, PagedAndSortedResultRequestDto, DeviceCreateUpdateDto>, IDeviceService
    {
        private readonly IRepository<Device, Guid> repository;

        public DeviceService(IRepository<Device, Guid> repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<List<DeviceDto>> GetByNodeId(Guid nodeId)
        {
            var queryable = await repository.GetQueryableAsync();
            var entities = queryable.Where(d => d.NodeId.CompareTo(nodeId) == 0).ToList();
            return await MapToGetListOutputDtosAsync(entities);
        }
    }
}
