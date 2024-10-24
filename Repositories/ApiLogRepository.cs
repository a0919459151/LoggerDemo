using Dapper;
using LoggerDemo.DbEntities;
using System.Data;

namespace LoggerDemo.Repositories
{
    public class ApiLogRepository
    {
        private readonly IDbConnection _dbConnection;

        public ApiLogRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void Create(ApiLog apiLog)
        {
            const string sql = @"
                INSERT INTO ApiLogs (Timestamp, Method, Endpoint, RequestHeaders, RequestBody, StatusCode, ResponseHeaders, ResponseBody, ExecutionTime)
                VALUES (@Timestamp, @Method, @Endpoint, @RequestHeaders, @RequestBody, @StatusCode, @ResponseHeaders, @ResponseBody, @ExecutionTime)";

            var parameters = new DynamicParameters();
            parameters.Add("@Timestamp", apiLog.Timestamp);
            parameters.Add("@Method", apiLog.Method);
            parameters.Add("@Endpoint", apiLog.Endpoint);
            parameters.Add("@RequestHeaders", apiLog.RequestHeaders);
            parameters.Add("@RequestBody", apiLog.RequestBody);
            parameters.Add("@StatusCode", apiLog.StatusCode);
            parameters.Add("@ResponseHeaders", apiLog.ResponseHeaders);
            parameters.Add("@ResponseBody", apiLog.ResponseBody);
            parameters.Add("@ExecutionTime", apiLog.ExecutionTime);

            _dbConnection.Execute(sql, parameters);
        }
    }
}
