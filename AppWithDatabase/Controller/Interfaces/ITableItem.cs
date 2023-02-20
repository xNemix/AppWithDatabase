namespace AppWithDatabase.Controller.Interfaces;

public interface ITableItem<T> where T : class
{
    Guid Id { get; set; }
}