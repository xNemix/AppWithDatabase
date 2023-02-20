using Dapper;
using System.Data.SqlClient;
using System.Reflection;

namespace AppWithDatabase.Controller.Repositories;

public class Repository<T> where T : class
{
    protected readonly SqlConnection Connection;

    public Repository()
    {
        Connection = new SqlConnection();
    }
    public Repository(SqlConnection connection)
    {
        Connection = connection;
    }

    public async Task<int> Create(T obj)
    {
        var type = typeof(T);
        var props = type.GetProperties();
        var sql = $"INSERT INTO {type.Name} ({GetParams(props)}) VALUES ({GetParams(props, true)})";
        return await Connection.ExecuteAsync(sql, obj);
    }

    public async Task<IEnumerable<T>> ReadAll()
    {
        var type = typeof(T);
        var props = type.GetProperties();
        var sql = $"SELECT Id, {GetParams(props)} FROM {type.Name}";
        return await Connection.QueryAsync<T>(sql);
    }

    public async Task<IEnumerable<T>> ReadOneById(int id)
    {
        var type = typeof(T);
        var props = type.GetProperties();
        var sql = $"SELECT Id, {GetParams(props)} FROM {type.Name} WHERE Id = @Id";
        return await Connection.QueryAsync<T>(sql, new { Id = id });
    }

    public async Task ReadFormattedTable()
    {
        var type = typeof(T);
        var props = type.GetProperties();
        var sql = $"SELECT Id, {GetParams(props)} FROM {type.Name}";
        var tables = await Connection.QueryAsync<T>(sql);


        Console.WriteLine($"Reading data from table {type.Name}...");
        var data = tables.GetEnumerator().Current;
        Console.WriteLine($"");
        Console.WriteLine($"Done Reading data from table {type.Name}..");
    }

    public async Task<int> Update(T obj)
    {
        var type = typeof(T);
        var props = type.GetProperties();
        var sql = $"UPDATE {type.Name} SET {GetSetters(props)} WHERE Id = @Id";
        return await Connection.ExecuteAsync(sql, obj);
    }

    public async Task<int> Delete(T obj)
    {
        var type = typeof(T);
        var sql = $"DELETE FROM {type.Name} WHERE Id = @Id";
        return await Connection.ExecuteAsync(sql, obj);
    }

    public override string ToString()
    {
        return typeof(T).Name;
    }


    protected static string GetParams(PropertyInfo[] props, bool includeAt = false)
    {
        // return string.Join(',', props.Where(p => p.Name != "Id").Select(p => (includeAt ? "@" : "") + p.Name)); Denne fungerer ikke!
        return string.Join(',', props.Where(p => p.Name != "Id").Select(p => includeAt ? "@" + p.Name : p.Name));
    }

    private static string GetSetters(PropertyInfo[] props)
    {
        return string.Join(',', props.Where(p => p.Name != "Id").Select(p => p.Name + " = @" + p.Name));
    }
}