using AppWithDatabase.Controller.Interfaces;

namespace AppWithDatabase.Model;

public class Users : ITableItem<Users>
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Users()
    {
        Id = Guid.NewGuid();
    }

    public Users(string name) : this()
    {
        Name = name;
    }
}