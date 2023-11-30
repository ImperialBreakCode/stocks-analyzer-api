using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Entities.OutboxEntities
{
	public class OutboxAcknowledgedMessageEntity
	{
		public string Id { get; set; }
		public string AcknowledgedInfo { get; set; }
		public DateTime AcknowledgedDateTime { get; set; }
	}
}
