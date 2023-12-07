using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Services.RabbitMQServices
{
	public class RabbitMQWalletDeletionService : IHostedService, IRabbitMQWalletDeletionService
	{
		private readonly IConnection _connection;
		private readonly IModel _channel;
		private readonly string _queueName = "walletDeleteQueue";
		public RabbitMQWalletDeletionService()
		{
			var factory = new ConnectionFactory { HostName = "localhost" };
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
		}
		public Task StartAsync(CancellationToken cancellationToken)
		{
			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (model, eventArgs) =>
			{
				var body = eventArgs.Body.ToArray();
				var walletId = Encoding.UTF8.GetString(body);
				
				DeleteWallet(walletId);
			};

			_channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_connection.Close();
			return Task.CompletedTask;
		}
		public void DeleteWallet(string walletId)
		{
			
		}
	}
}
