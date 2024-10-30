using Dapper;
using LoggerDemo.DbEntities;
using System.Collections.Generic;
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

        public void CreateDog(Dog dog, IDbTransaction dbTransaction = null)
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

        public void CreateDogs(IEnumerable<Dog> dogs, IDbTransaction dbTransaction = null)
        {
            foreach (var dog in dogs)
            {
                CreateDog(dog);
            }
        }
    }
}
