using System.Collections.Generic;
using Iot.Greenhouse.Nodes;

namespace Iot.Greenhouse.Blazor.Pages;

public partial class Dashboard
{
    public class NodeViewModel
    {
        public NodeDto Node { get; set; } = default!;
        public bool IsOnline { get; set; }
        public List<SensorViewModel>? Sensors { get; set; }
        public List<DeviceViewModel>? Devices { get; set; }
        public NodeViewModel()
        {
        }
    }
}