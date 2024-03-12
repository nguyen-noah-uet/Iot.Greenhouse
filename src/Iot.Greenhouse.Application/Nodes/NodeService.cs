using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Iot.Greenhouse.Nodes
{
    public class NodeService :
        CrudAppService<Node, NodeDto, int, PagedAndSortedResultRequestDto, NodeCreateUpdateDto>,
        INodeService
    {
        public NodeService(IRepository<Node, int> repository) : base(repository)
        {
        }
    }
}
