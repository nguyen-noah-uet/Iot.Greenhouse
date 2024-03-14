using Iot.Greenhouse.Devices;
using Iot.Greenhouse.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Iot.Greenhouse.Nodes
{
    public class Node : AggregateRoot<Guid>
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = default!;
        [MaxLength(200)]
        public string? Description { get; set; } = default!;

        public ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}
