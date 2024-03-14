using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Iot.Greenhouse.Devices
{
    public class DeviceStatus: CreationAuditedEntity<int>
    {
        public Guid DeviceId { get; set; }
        public Device Device { get; set; } = default!;
        public bool IsOn { get; set; }
    }
}
