using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace DotnetAPI
{
  class DataContextDapper
  {
    private readonly IConfiguration _config;

    public DataContextDapper(IConfiguration config)
    {
      _config = config;
    }

    public IEnumerable<T> LoadData<T>(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
      return dbConnection.Query<T>(sql);
    }

    public T LoadDataSingle<T>(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
      return dbConnection.QuerySingle<T>(sql);
    }

    public bool ExecuteSql(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
      return dbConnection.Execute(sql) > 0;
    }

    public int ExecuteSqlWithRowCount(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
      return dbConnection.Execute(sql);
    }

    public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
    {
      var sqlCommand = new SqlCommand(sql);
      foreach (var p in parameters)
      {
        sqlCommand.Parameters.Add(p);
      }

      var dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
      dbConnection.Open();
      sqlCommand.Connection = dbConnection;
      int rowsAffected = sqlCommand.ExecuteNonQuery();
      dbConnection.Close();

      return rowsAffected > 0;
    }
    public IEnumerable<T> LoadDataWithParams<T>(string sql, DynamicParameters parameters)
    {
      IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
      return dbConnection.Query<T>(sql, parameters);
    }

    public T LoadDataSingleWithParams<T>(string sql, DynamicParameters parameters)
    {
      IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
      return dbConnection.QuerySingle<T>(sql, parameters);
    }

  }
}
