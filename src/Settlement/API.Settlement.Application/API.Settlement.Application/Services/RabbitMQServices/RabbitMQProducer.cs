using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Services.RabbitMQServices
{
	public class RabbitMQProducer : IRabbitMQProducer
	{
		public void SendMessage(OutboxPendingMessage outboxPendingMessageEntity)
		{
			var factory = new ConnectionFactory { HostName = "localHost" };
			var connection = factory.CreateConnection();
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(outboxPendingMessageEntity.QueueType, durable:true, exclusive: false);
				var json = outboxPendingMessageEntity.Body;
				var body = Encoding.UTF8.GetBytes(json);
				channel.BasicPublish(exchange: "", routingKey: outboxPendingMessageEntity.QueueType, body: body);
			}
		}


	}
}
