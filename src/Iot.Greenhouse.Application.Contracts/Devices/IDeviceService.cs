using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Iot.Greenhouse.Devices
{
    public interface IDeviceService : ICrudAppService<DeviceDto, Guid, PagedAndSortedResultRequestDto, DeviceCreateUpdateDto>
    {
        Task<List<DeviceDto>> GetByNodeId(Guid nodeId);
    }

    public class DeviceCreateUpdateDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public Guid NodeId { get; set; }
        public DeviceType DeviceType { get; set; }
    }

    public class DeviceDto : EntityDto<Guid>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public Guid NodeId { get; set; }
        public DeviceType DeviceType { get; set; }
    }
}
