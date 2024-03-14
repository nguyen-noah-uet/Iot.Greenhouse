using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iot.Greenhouse.Devices;
using Iot.Greenhouse.Nodes;
using Iot.Greenhouse.Sensors;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Iot.Greenhouse.Data
{
    public class GreenhouseDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly ILogger<GreenhouseDataSeeder> logger;
        private readonly IRepository<Node, Guid> nodeRepository;
        private readonly IRepository<Device, Guid> deviceRepository;
        private readonly IRepository<Sensor, Guid> sensorRepository;
        private readonly IRepository<NodeStatus, int> nodeStatusRepository;
        private readonly IRepository<DeviceStatus, int> deviceStatusRepository;
        private readonly IRepository<SensorData, int> sensorDataRepository;

        public GreenhouseDataSeeder( 
            ILogger<GreenhouseDataSeeder> logger,
            IRepository<Node, Guid> nodeRepository,
            IRepository<Device, Guid> deviceRepository,
            IRepository<Sensor, Guid> sensorRepository,
            IRepository<NodeStatus, int> nodeStatusRepository,
            IRepository<DeviceStatus, int> deviceStatusRepository,
            IRepository<SensorData, int> sensorDataRepository)
        {
            this.logger = logger;
            this.nodeRepository = nodeRepository;
            this.deviceRepository = deviceRepository;
            this.sensorRepository = sensorRepository;
            this.nodeStatusRepository = nodeStatusRepository;
            this.deviceStatusRepository = deviceStatusRepository;
            this.sensorDataRepository = sensorDataRepository;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            if (await nodeRepository.GetCountAsync() > 0)
                return;
            // Seed nodes
            var controlDeviceNode = await nodeRepository.InsertAsync(new Node()
            {
                Name = $"Control node",
                Description = "Node for control pump devices",
            });
            logger.LogInformation("Inserted {@node}", controlDeviceNode);
            var moistureNode = await nodeRepository.InsertAsync(new Node()
            {
                Name = $"Sensor node 1",
                Description = "Node for moisture sensors",
            });
            logger.LogInformation("Inserted {@node}", moistureNode);
            var PhEcNode = await nodeRepository.InsertAsync(new Node()
            {
                Name = $"Sensor node 2",
                Description = "Node for pH and EC sensors",
            });
            logger.LogInformation("Inserted {@node}", PhEcNode);

            // Seed devices
            var waterPump = await deviceRepository.InsertAsync(new Device()
            {
                Name = "Water pump",
                Description = "Pump water to plants",
                NodeId = controlDeviceNode.Id,
                DeviceType = DeviceType.WaterPump
            });
            logger.LogInformation("Inserted {@device}", waterPump);
            var micronutrientsPump = await deviceRepository.InsertAsync(new Device()
            {
                Name = "Micronutrients pump",
                Description = "Pump micronutrients to plants",
                NodeId = controlDeviceNode.Id,
                DeviceType = DeviceType.WaterPump
            });
            logger.LogInformation("Inserted {@device}", micronutrientsPump);
            var macronutrientsPump1 = await deviceRepository.InsertAsync(new Device()
            {
                Name = "Macronutrients pump 1",
                Description = "Pump macronutrients to plants",
                NodeId = controlDeviceNode.Id,
                DeviceType = DeviceType.WaterPump
            });
            logger.LogInformation("Inserted {@device}", macronutrientsPump1);
            var macronutrientsPump2 = await deviceRepository.InsertAsync(new Device()
            {
                Name = "Macronutrients pump 2",
                Description = "Pump macronutrients to plants",
                NodeId = controlDeviceNode.Id,
                DeviceType = DeviceType.WaterPump
            });
            logger.LogInformation("Inserted {@device}", macronutrientsPump2);
            
            var light = await deviceRepository.InsertAsync(new Device()
            {
                Name = "Light",
                Description = "Provide light to plants",
                NodeId = controlDeviceNode.Id,
                DeviceType = DeviceType.Light
            });
            logger.LogInformation("Inserted {@device}", light);
            var fan = await deviceRepository.InsertAsync(new Device()
            {
                Name = "Fan",
                Description = "Provide air circulation to plants",
                NodeId = controlDeviceNode.Id,
                DeviceType = DeviceType.Fan
            });
            logger.LogInformation("Inserted {@device}", fan);

            // Seed sensors
            var moistureSensor1 = await sensorRepository.InsertAsync(new Sensor()
            {
                Name = "Moisture sensor",
                Description = "Measure moisture in soil",
                NodeId = moistureNode.Id,
                SensorType = SensorType.SoilMoisture,
                Unit = "%"
            });
            logger.LogInformation("Inserted {@sensor}", moistureSensor1);
            var pHsensor = await sensorRepository.InsertAsync(new Sensor()
            {
                Name = "pH sensor",
                Description = "Measure pH in water",
                NodeId = PhEcNode.Id,
                SensorType = SensorType.Ph,
                Unit = null
            });
            logger.LogInformation("Inserted {@sensor}", pHsensor);
            var ECsensor = await sensorRepository.InsertAsync(new Sensor()
            {
                Name = "EC sensor",
                Description = "Measure EC in water",
                NodeId = PhEcNode.Id,
                SensorType = SensorType.Ec,
                Unit = "μS/cm"
            });
            logger.LogInformation("Inserted {@sensor}", ECsensor);
        }
    }
}
