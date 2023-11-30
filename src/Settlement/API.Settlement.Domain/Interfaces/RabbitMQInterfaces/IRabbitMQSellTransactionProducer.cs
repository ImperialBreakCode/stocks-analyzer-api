using API.Settlement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.RabbitMQInterfaces
{
	public interface IRabbitMQSellTransactionProducer
	{
		void SendMessage(Transaction message);
	}
}
