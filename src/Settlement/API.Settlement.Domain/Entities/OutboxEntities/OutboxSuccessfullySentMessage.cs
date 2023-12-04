using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Entities.OutboxEntities
{
	public class OutboxSuccessfullySentMessage
	{
		public string Id { get; set; }
		public string QueueType { get; set; }
		public string SentInfo { get; set; }
		public DateTime SentDateTime { get; set; }
	}
}
