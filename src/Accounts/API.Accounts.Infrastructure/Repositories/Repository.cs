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

        public Repository(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            _connection = sqlConnection;
            _transaction = sqlTransaction;

            _insertQuery = SqlQueryGeneratorHelper.GenerateInsertQuery<T>();
            _updateQuery = SqlQueryGeneratorHelper.GenerateUpdateQuery<T>();
        }

        protected string InsertQuery => _insertQuery;
        protected string UpdateQuery => _updateQuery;

        public void Delete(string id)
        {
            string query = $"DELETE FROM {typeof(T).Name} WHERE Id=@id";
            var command = CreateCommand(query, true);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public ICollection<T> GetManyByCondition(Func<T, bool> condition)
        {
            throw new NotImplementedException();
        }

        public T GetOneById(string id)
        {
            string query = $"SELECT * FROM {typeof(T).Name} WHERE Id=@id";
            var command = CreateCommand(query, false);
            command.Parameters.AddWithValue("@id", id);

            T entity = (T)Activator.CreateInstance(typeof(T))!;

            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                EntityConverterHelper.ToEntity(ref entity, reader);
            }

            return entity;
        }

        public void Insert(T entity)
        {
            var command = CreateCommand(InsertQuery, true);

            EntityConverterHelper.ToQuery(entity, command);

            command.ExecuteNonQuery();
        }

        public void Update(T entity)
        {
            var command = CreateCommand(UpdateQuery, true);

            EntityConverterHelper.ToQuery(entity, command);

            command.Parameters.AddWithValue("@id", entity.Id);
            command.ExecuteNonQuery();
        }

        protected SqlCommand CreateCommand(string query, bool useTransaction)
        {
            SqlCommand sqlCommand = _connection.CreateCommand();
            sqlCommand.CommandText = query;
            
            if (useTransaction)
            {
                sqlCommand.Transaction = _transaction;
            }

            return sqlCommand;
        }
    }
}
