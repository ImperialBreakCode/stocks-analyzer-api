using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.RabbitMqServices
{
	public class RabbitMQSellTransactionProducer : IRabbitMQSellTransactionProducer
	{
		public void SendMessage(Transaction message)
		{
			var factory = new ConnectionFactory { HostName = "localHost" };
			var connection = factory.CreateConnection();
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare("transactionSellStock", exclusive: false);
				var json = JsonConvert.SerializeObject(message);
				var body = Encoding.UTF8.GetBytes(json);
				channel.BasicPublish(exchange: "", routingKey: "transactionSellStock", body: body);
			}
		}


	}
}
