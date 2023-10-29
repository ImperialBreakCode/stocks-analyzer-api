using API.Accounts.Domain.Interfaces;
using API.Accounts.Infrastructure.Helpers;
using Microsoft.Data.SqlClient;

namespace API.Accounts.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : IEntity
    {
        private readonly SqlTransaction _transaction;
        private readonly SqlConnection _connection;

        private readonly string _insertQuery;
        private readonly string _updateQuery;
        private readonly string _getByIdQuery;
        private readonly string _getAllQuery;
        private readonly string _deleteByIdQuery;


        public Repository(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            _connection = sqlConnection;
            _transaction = sqlTransaction;

            _insertQuery = SqlQueryGeneratorHelper.GenerateInsertQuery<T>();
            _updateQuery = SqlQueryGeneratorHelper.GenerateUpdateQuery<T>();
            _deleteByIdQuery = $"DELETE FROM [{typeof(T).Name}] WHERE Id=@id";
            _getByIdQuery = $"SELECT * FROM [{typeof(T).Name}] WHERE Id=@id";
            _getAllQuery = $"SELECT * FROM [{typeof(T).Name}]";
        }

        protected string InsertQuery => _insertQuery;
        protected string UpdateQuery => _updateQuery;
        protected string GetByIdQuery => _getByIdQuery;
        protected string GetAllQuery => _getAllQuery;
        protected string DeleteByIdQuery => _deleteByIdQuery;

        public void Delete(string id)
        {
            var command = CreateCommand(DeleteByIdQuery);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public T? GetOneById(string id)
        {
            var command = CreateCommand(GetByIdQuery);
            command.Parameters.AddWithValue("@id", id);

            return EntityConverterHelper.ToEntityCollection<T>(command).FirstOrDefault();
        }

        public void Insert(T entity)
        {
            var command = CreateCommand(InsertQuery);

            EntityConverterHelper.ToQuery(entity, command);

            command.ExecuteNonQuery();
        }

        public void Update(T entity)
        {
            var command = CreateCommand(UpdateQuery);

            EntityConverterHelper.ToQuery(entity, command);

            command.Parameters.AddWithValue("@id", entity.Id);
            command.ExecuteNonQuery();
        }

        public ICollection<T> GetAll()
        {
            var command = CreateCommand(GetAllQuery);
            return EntityConverterHelper.ToEntityCollection<T>(command);
        }

        public ICollection<T> GetManyByCondition(Func<T, bool> condition)
        {
            return GetAll().Where(condition).ToList();
        }

        protected SqlCommand CreateCommand(string query)
        {
            SqlCommand sqlCommand = _connection.CreateCommand();
            sqlCommand.CommandText = query;
            sqlCommand.Transaction = _transaction;

            return sqlCommand;
        }
    }
}
