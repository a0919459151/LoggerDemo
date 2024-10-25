using Dapper;
using LoggerDemo.DbEntities;
using System.Data;

namespace LoggerDemo.Repositories
{
    public class CatRepository
    {
        private readonly IDbConnection _dbConnection;

        public CatRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void Create(Cat cat, IDbTransaction dbTransaction = null)
        {
            const string sql = @"
                INSERT INTO Cats 
                    (Name)
                VALUES 
                    (@Name)";

            var parameters = new DynamicParameters();
            parameters.Add("@Name", cat.Name);
          
            _dbConnection.Execute(sql, parameters, dbTransaction);
        }
    }
}
