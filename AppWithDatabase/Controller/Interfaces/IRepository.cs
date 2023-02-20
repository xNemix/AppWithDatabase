namespace AppWithDatabase.Controller.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<int> Create(T entity);
    Task<IEnumerable<T>> Read(Guid id);

    void Update(T entity);
    void Delete(T entity);
}