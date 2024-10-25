using Dapper;
using LoggerDemo.DbEntities;
using System.Data;

namespace LoggerDemo.Repositories
{
    public class DogRepository
    {
        private readonly IDbConnection _dbConnection;

        public DogRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void Create(Dog dog, IDbTransaction dbTransaction = null)
        {
            const string sql = @"
                INSERT INTO Dogs 
                    (Name)
                VALUES 
                    (@Name)";

            var parameters = new DynamicParameters();
            parameters.Add("@Name", dog.Name);

            _dbConnection.Execute(sql, parameters, dbTransaction);
        }
    }
}
