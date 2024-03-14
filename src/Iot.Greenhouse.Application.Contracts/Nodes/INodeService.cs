using Iot.Greenhouse.Devices;
using Iot.Greenhouse.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Iot.Greenhouse.Nodes
{
    public interface INodeService : ICrudAppService<NodeDto, Guid, PagedAndSortedResultRequestDto, NodeCreateUpdateDto>
    {
        Task<List<NodeDto>> GetAllNodesList();
    }
    public class NodeDto : EntityDto<Guid>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public List<SensorDto> Sensors { get; set; } = default!;
        public List<DeviceDto> Devices { get; set; } = default!;
    }

    public class NodeCreateUpdateDto
    {
        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
