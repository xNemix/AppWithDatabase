using System;
using System.Collections.Generic;

namespace AppWithDatabase.Models;

public partial class Sensor
{
    public long Id { get; set; }

    public string SensorName { get; set; } = null!;

    public string SensorLocation { get; set; } = null!;
}
