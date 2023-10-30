using Microsoft.Data.SqlClient;

namespace API.Accounts.Infrastructure.Helpers
{
    public static class EntityConverterHelper
    {
        public static ICollection<T> ToEntityCollection<T>(SqlCommand command)
        {
            var collection = new List<T>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    T entity = (T)Activator.CreateInstance(typeof(T))!;
                    ToEntity(entity, reader);
                    collection.Add(entity);
                }
            }

            return collection;
        }

        public static T ToEntity<T>(T entity, SqlDataReader reader)
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
