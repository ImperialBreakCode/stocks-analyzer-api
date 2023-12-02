using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Entities.OutboxEntities
{
	public class OutboxPendingMessageEntity
	{
		public string Id { get; set; }
		public string QueueType { get; set; }
		public string Body { get; set; }
		public DateTime PendingDateTime { get; set; }
	}
}
