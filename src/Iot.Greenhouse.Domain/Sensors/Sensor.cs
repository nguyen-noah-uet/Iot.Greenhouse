using Iot.Greenhouse.Nodes;
using Volo.Abp.Domain.Entities.Auditing;

namespace Iot.Greenhouse.Sensors
{
    public class Sensor : CreationAuditedAggregateRoot<int>
    {
        public SensorType SensorType { get; set; }
        public double SensorValue { get; set; }
        public int NodeId { get; set; }
        public Node Node { get; set; } = default!;
    }
}
