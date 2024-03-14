using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Iot.Greenhouse.Nodes
{
    public interface INodeStatusService: ICrudAppService<NodeStatusDto, int, PagedAndSortedResultRequestDto, NodeStatusCreateDto>
    {
    }

    public class NodeStatusCreateDto
    {
        public Guid NodeId { get; set; } = default!;
        public bool IsOnline { get; set; }
    }

    public class NodeStatusDto : CreationAuditedEntityDto<int>
    {
        public Guid NodeId { get; set; } = default!;
        public bool IsOnline { get; set; }
    }
}
