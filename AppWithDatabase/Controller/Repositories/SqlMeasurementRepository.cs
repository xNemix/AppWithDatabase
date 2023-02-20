using AppWithDatabase.Model;
using Dapper;
using System.Data.SqlClient;

namespace AppWithDatabase.Controller.Repositories;

public class SqlMeasurementRepository : SqlRepository<Measurement>
{
    public SqlMeasurementRepository(string connectionString) : base(connectionString)
    {
    }

    public override async Task<IEnumerable<Measurement>> GetAll()
    {
        const string sql = "SELECT * FROM Measurements";
        return await Connection.QueryAsync<Measurement>(sql);
    }

    public override async Task<int> Create(Measurement measurement)
    {
        const string sql = "INSERT INTO Measurements VALUES (@Id, @UserId, @SensorId, @Temperature)";
        return await Connection.ExecuteAsync(sql, measurement);
    }

    public override async Task<IEnumerable<Measurement>> Read(Guid id)
    {
        var props = GetProps();
        var sql = $"SELECT {GetParams(props)} FROM Measurements WHERE Id = @Id";

        return await Connection.QueryAsync<Measurement>(sql, new { Id = id });
    }


    public override void Update(Measurement entity)
    {
        var props = GetProps();
        var sql = $"UPDATE Measurements SET {GetSetters(props)} WHERE Id = @Id";
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        connection.Execute(sql, entity);
    }

    public override void Delete(Measurement entity)
    {
        const string sql = "DELETE FROM Measurements WHERE Id = @Id";
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        connection.Execute(sql, entity);
    }
}