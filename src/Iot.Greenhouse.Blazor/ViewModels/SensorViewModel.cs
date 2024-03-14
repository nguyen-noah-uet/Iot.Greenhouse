using Iot.Greenhouse.Sensors;

namespace Iot.Greenhouse.Blazor.Pages;

public partial class Dashboard
{
    public class SensorViewModel {
        public SensorDto Sensor { get; set; }
        public double CurrentValue { get; set; } = double.NaN;
    }
}