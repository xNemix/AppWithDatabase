using AppWithDatabase.Controller.Interfaces;

namespace AppWithDatabase.Model;

public class Measurement : ITableItem<Measurement>
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public int SensorId { get; set; }
    public float Temperature { get; set; }

    public Measurement()
    {
        Id = Guid.NewGuid();
    }

    public Measurement(int userId, int sensorId, float temperature) : this()
    {
        UserId = userId;
        SensorId = sensorId;
        Temperature = temperature;
    }
}