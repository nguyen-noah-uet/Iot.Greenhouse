using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Iot.Greenhouse.Nodes
{
    public class NodeStatusService : CrudAppService<NodeStatus, NodeStatusDto, int, PagedAndSortedResultRequestDto, NodeStatusCreateDto>, INodeStatusService
    {
        public NodeStatusService(IRepository<NodeStatus, int> repository) : base(repository)
        {
        }
    }
}
