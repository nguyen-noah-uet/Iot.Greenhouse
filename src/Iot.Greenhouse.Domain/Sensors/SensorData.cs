using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Iot.Greenhouse.Sensors
{
    public class SensorData: CreationAuditedEntity<int>
    {
        public Guid SensorId { get; set; }
        public Sensor Sensor { get; set; } = default!;
        public double Value { get; set; }
    }
}
