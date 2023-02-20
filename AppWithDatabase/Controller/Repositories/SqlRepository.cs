using AppWithDatabase.Controller.Interfaces;
using System.Data.SqlClient;
using System.Reflection;

namespace AppWithDatabase.Controller.Repositories;

public abstract class SqlRepository<T> : IRepository<T> where T : class
{
    protected readonly string ConnectionString;
    protected readonly SqlConnection Connection;


    protected SqlRepository(string connectionString)
    {
        ConnectionString = connectionString;
        Connection = new SqlConnection(connectionString);
    }

    public abstract Task<IEnumerable<T>> GetAll();

    public abstract Task<int> Create(T entity);
    public abstract Task<IEnumerable<T>> Read(Guid id);
    public abstract void Update(T entity);

    public abstract void Delete(T entity);


    protected static PropertyInfo[] GetProps()
    {
        var type = typeof(T);
        var props = type.GetProperties();
        return props;
    }

    protected static string GetParams(PropertyInfo[] props, bool includeAt = false)
    {
        // return string.Join(',', props.Where(p => p.Name != "Id").Select(p => (includeAt ? "@" : "") + p.Name)); Denne fungerer ikke!
        return string.Join(',', props.Where(p => p.Name != "Id").Select(p => includeAt ? "@" + p.Name : p.Name));
    }

    protected static string GetSetters(PropertyInfo[] props)
    {
        return string.Join(',', props.Where(p => p.Name != "Id").Select(p => p.Name + " = @" + p.Name));
    }

}