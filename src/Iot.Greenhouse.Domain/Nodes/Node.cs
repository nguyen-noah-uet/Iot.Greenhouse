using Volo.Abp.Domain.Entities;

namespace Iot.Greenhouse.Nodes
{
    public class Node : AggregateRoot<int>
    {
        public string NodeName { get; set; } = default!;
    }
}
