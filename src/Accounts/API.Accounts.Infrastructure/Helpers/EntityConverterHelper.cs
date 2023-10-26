using Microsoft.Data.SqlClient;

namespace API.Accounts.Infrastructure.Helpers
{
    public static class EntityConverterHelper
    {
        public static T ToEntity<T>(ref T entity, SqlDataReader reader)
        {
            var properties = entity.GetType().GetProperties();

            foreach (var property in properties)
            {
                property.SetValue(entity, reader[property.Name]);
            }

            return entity;
        }

        public static void ToQuery<T>(T entity, SqlCommand command)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(entity));
            }
        }
    }
}
