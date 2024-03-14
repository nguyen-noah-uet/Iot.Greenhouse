using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Iot.Greenhouse.Nodes
{
    public class NodeService :
        CrudAppService<Node, NodeDto, Guid, PagedAndSortedResultRequestDto, NodeCreateUpdateDto>,
        INodeService
    {
        private readonly IRepository<Node, Guid> repository;
        public NodeService(IRepository<Node, Guid> repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<List<NodeDto>> GetAllNodesList()
        {
            var queryable = await repository.GetQueryableAsync();
            // include sensors and devices
            var entities = queryable.ToList();
            return await MapToGetListOutputDtosAsync(entities);
        }
    }
}
