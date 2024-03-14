using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Iot.Greenhouse.Devices
{
    public interface IDeviceStatusService : ICrudAppService<DeviceStatusDto,int,PagedAndSortedResultRequestDto, DeviceStatusCreateDto>
    {
    }

    public class DeviceStatusCreateDto
    {
        public Guid DeviceId { get; set; } = default!;
        public bool IsOn { get; set; }
    }

    public class DeviceStatusDto : CreationAuditedEntityDto<int>
    {
        public Guid DeviceId { get; set; } = default!;
        public bool IsOn { get; set; }
    }
}
