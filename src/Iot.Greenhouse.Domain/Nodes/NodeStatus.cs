using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Iot.Greenhouse.Nodes
{
    public class NodeStatus : CreationAuditedEntity<int>
    {
        public Guid NodeId { get; set; }
        public Node Node { get; set; } = default!;
        public bool IsOnline { get; set; }
    }
}
