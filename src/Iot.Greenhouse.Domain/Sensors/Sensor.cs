﻿using Iot.Greenhouse.Nodes;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Iot.Greenhouse.Sensors
{
    public class Sensor : AggregateRoot<Guid>
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = default!;
        [MaxLength(200)]
        public string? Description { get; set; } = default!;
        public Guid NodeId { get; set; }
        public Node Node { get; set; } = default!;
        public SensorType SensorType { get; set; }
        [MaxLength(20)]
        public string? Unit { get; set; }
    }
}
