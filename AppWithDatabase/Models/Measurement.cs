using System;
using System.Collections.Generic;

namespace AppWithDatabase.Models;

public partial class Measurement
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long SensorId { get; set; }

    public double Temperature { get; set; }
}
