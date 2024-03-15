using Iot.Greenhouse.Sensors;
using System.Collections.Generic;

namespace Iot.Greenhouse.Blazor.Pages;

public partial class Dashboard
{
    public class SensorViewModel {
        public SensorDto Sensor { get; set; }
        public double CurrentValue { get; set; } = double.NaN;
        public List<SensorDataDto>? Data { get; set; } = default;
    }
}