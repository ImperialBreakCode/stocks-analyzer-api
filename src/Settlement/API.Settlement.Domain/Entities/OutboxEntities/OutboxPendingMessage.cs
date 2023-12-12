namespace API.Settlement.Domain.Entities.OutboxEntities
{
	public class OutboxPendingMessage
	{
		public string Id { get; set; }
		public string QueueType { get; set; }
		public string Body { get; set; }
		public DateTime PendingDateTime { get; set; }
	}
}
