using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Iot.Greenhouse.Nodes
{
    public interface INodeService : ICrudAppService<NodeDto, int, PagedAndSortedResultRequestDto, NodeCreateUpdateDto>
    {

    }
    public class NodeDto : EntityDto<int>
    {
        public string NodeName { get; set; } = default!;
    }

    public class NodeCreateUpdateDto
    {
        [Required]
        public string NodeName { get; set; } = default!;
    }
}
