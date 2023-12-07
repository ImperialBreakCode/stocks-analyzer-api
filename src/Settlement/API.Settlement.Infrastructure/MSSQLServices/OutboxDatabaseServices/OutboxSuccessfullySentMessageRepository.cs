using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.OutboxDatabaseInterfaces;
using Microsoft.Data.SqlClient;

namespace API.Settlement.Infrastructure.MSSQLServices.OutboxDatabaseServices
{
    public class OutboxSuccessfullySentMessageRepository : IOutboxSuccessfullySentMessageRepository
    {
        private readonly SqlConnection _connection;

        public OutboxSuccessfullySentMessageRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public void AddSuccessfullySentMessage(OutboxSuccessfullySentMessage outboxAcknowledgedMessageEntity)
        {
            string commandText = $@"INSERT INTO SuccessfullySentMessage
								(Id, QueueType, SentInfo, SentDateTime) VALUES
								(@Id, @QueueType, @SentInfo, @SentDateTime)";

            using (SqlCommand command = new SqlCommand(commandText, _connection))
            {
                _connection.Open();
                command.Parameters.AddWithValue("@Id", outboxAcknowledgedMessageEntity.Id);
                command.Parameters.AddWithValue("@QueueType", outboxAcknowledgedMessageEntity.QueueType);
                command.Parameters.AddWithValue("@SentInfo", outboxAcknowledgedMessageEntity.SentInfo);
                command.Parameters.AddWithValue("@SentDateTime", outboxAcknowledgedMessageEntity.SentDateTime);
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

    }
}
