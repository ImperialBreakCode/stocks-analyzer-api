using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.SQLiteInterfaces.TransactionDatabaseInterfaces;
using API.Settlement.Domain.Interfaces.RabbitMQInterfaces;
using Microsoft.Extensions.DependencyInjection;
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
	public class RabbitMQWalletDeletionService : IRabbitMQWalletDeletionService, IHostedService
	{
		private readonly IConnection _connection;
		private readonly IModel _channel;
		private readonly string _queueName = "walletDeleteQueue";
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public RabbitMQWalletDeletionService(IServiceScopeFactory serviceScopeFactory)
		{
			var factory = new ConnectionFactory { HostName = "localhost" };
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
			_serviceScopeFactory = serviceScopeFactory;
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
			using (var scope = _serviceScopeFactory.CreateScope())
			{
				var _walletService = scope.ServiceProvider.GetRequiredService<IWalletService>();
				var _transactionDbService = scope.ServiceProvider.GetRequiredService<ITransactionDatabaseService>();
				_walletService.DeleteWallet(walletId);
				_transactionDbService.DeleteTransactionsWithWalletId(walletId);
			}

		}
	}
}
