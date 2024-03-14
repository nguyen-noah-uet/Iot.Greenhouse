using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Iot.Greenhouse.Devices
{
    public class DeviceStatusService : CrudAppService<DeviceStatus, DeviceStatusDto, int, PagedAndSortedResultRequestDto, DeviceStatusCreateDto>, IDeviceStatusService
    {
        public DeviceStatusService(IRepository<DeviceStatus, int> repository) : base(repository)
        {
        }

    }
}
