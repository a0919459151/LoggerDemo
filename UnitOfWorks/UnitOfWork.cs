using System.Data;
using System;

namespace LoggerDemo.UnitOfWorks
{
    public class UnitOfWork
    {
        private readonly object _beginTransactionLock = new object();
        private IDbTransaction _transaction;

        private readonly IDbConnection _dbConnection;

        public UnitOfWork(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IDbTransaction BeginTransaction()
        {
            // Ensure thread safety
            lock (_beginTransactionLock)
            {
                // Check if a transaction is already in progress
                if (_transaction != null)
                {
                    throw new InvalidOperationException("A transaction is already in progress.");
                }

                var isOpen = _dbConnection.State == ConnectionState.Open;
                if (!isOpen)
                {
                    _dbConnection.Open();
                }
                _transaction = _dbConnection.BeginTransaction();
                return _transaction;
            }
        }

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
        }
    }
}
