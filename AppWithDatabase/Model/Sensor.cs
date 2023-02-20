using AppWithDatabase.Controller.Interfaces;

namespace AppWithDatabase.Model;

public class Sensor : ITableItem<Sensor>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }

    public Sensor()
    {
        Id = Guid.NewGuid();
    }

    public Sensor(string name, string location) : this()
    {
        Name = name;
        Location = location;
    }
}