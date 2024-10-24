using Dapper;
using LoggerDemo.DbEntities;
using System.Data;

namespace LoggerDemo.Repositories
{
    public class HttpClientLogRepository
    {
        private readonly IDbConnection _dbConnection;

        public HttpClientLogRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void Create(HttpClientLog httpClientLog)
        {
            const string sql = @"
                INSERT INTO HttpClientLogs (Timestamp, Method, Endpoint, RequestHeaders, RequestBody, StatusCode, ResponseHeaders, ResponseBody, ExecutionTime)
                VALUES (@Timestamp, @Method, @Endpoint, @RequestHeaders, @RequestBody, @StatusCode, @ResponseHeaders, @ResponseBody, @ExecutionTime)";

            var parameters = new DynamicParameters();
            parameters.Add("@Timestamp", httpClientLog.Timestamp);
            parameters.Add("@Method", httpClientLog.Method);
            parameters.Add("@Endpoint", httpClientLog.Endpoint);
            parameters.Add("@RequestHeaders", httpClientLog.RequestHeaders);
            parameters.Add("@RequestBody", httpClientLog.RequestBody);
            parameters.Add("@StatusCode", httpClientLog.StatusCode);
            parameters.Add("@ResponseHeaders", httpClientLog.ResponseHeaders);
            parameters.Add("@ResponseBody", httpClientLog.ResponseBody);
            parameters.Add("@ExecutionTime", httpClientLog.ExecutionTime);

            _dbConnection.Execute(sql, parameters);
        }
    }
}
