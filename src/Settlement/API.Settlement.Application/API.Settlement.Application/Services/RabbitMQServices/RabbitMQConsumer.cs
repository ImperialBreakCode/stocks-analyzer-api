using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace API.Settlement.Application.Services.RabbitMQServices
{
	public class RabbitMQConsumer : IRabbitMQConsumer
	{
		public string ConsumeMessage(string queue)
		{
			string message = string.Empty;

			var factory = new ConnectionFactory { HostName = "localhost" };
			var connection = factory.CreateConnection();
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue);
				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, eventArgs) =>
				{
					var body = eventArgs.Body.ToArray();
					message = Encoding.UTF8.GetString(body);
				};
				channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
			}

			return message;
		}
	}
}
