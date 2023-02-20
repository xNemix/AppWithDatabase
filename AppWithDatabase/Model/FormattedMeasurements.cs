using AppWithDatabase.Controller.Interfaces;

namespace AppWithDatabase.Model;

public class FormattedMeasurements : ITableItem<FormattedMeasurements>
{
    public Guid Id { get; set; }
    public string? SensorName { get; set; }
    public string? SensorLocation { get; set; }
    public float SensorTemperature { get; set; }
    public string? SensorUser { get; set; }

    public FormattedMeasurements()
    {
        Id = Guid.NewGuid();
    }

    public FormattedMeasurements(string? sensorName, string? sensorLocation, float sensorTemperature, string? sensorUser) : this()
    {
        SensorName = sensorName;
        SensorLocation = sensorLocation;
        SensorTemperature = sensorTemperature;
        SensorUser = sensorUser;
    }
}