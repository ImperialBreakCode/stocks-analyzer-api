using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Entities.OutboxEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.RabbitMQInterfaces
{
	public interface IRabbitMQProducer
	{
		void SendMessage(OutboxPendingMessage outboxPendingMessageEntity);
	}
}
