using Iot.Greenhouse.Devices;

namespace Iot.Greenhouse.Blazor.Pages;

public partial class Dashboard
{
    public class DeviceViewModel
    {
        public DeviceDto Device { get; set; } = default!;
        public bool IsOn { get; set; }
    }
}