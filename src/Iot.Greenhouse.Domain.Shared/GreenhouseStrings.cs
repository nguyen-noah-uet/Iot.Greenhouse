namespace Iot.Greenhouse
{
    public static class GreenhouseStrings
    {
        public static class Topics
        {
            public const string NodeStatus = "node-status/";
            public const string Sensors = "sensors/";
            public const string DeviceStatus = "device-status/";
            public const string Command = "command/";
            public const string Notifications = "notifications/";
        }
        public static class Devices
        {
            public const string Pump = "Pump";
            public const string Light = "Light";
            public const string Fan = "Fan";
        }
        public static class Sensors
        {
            public const string Temperature = "Temperature";
            public const string Humidity = "Humidity";
            public const string Ec = "EC";
            public const string Ph = "pH";
        }

    }
}
