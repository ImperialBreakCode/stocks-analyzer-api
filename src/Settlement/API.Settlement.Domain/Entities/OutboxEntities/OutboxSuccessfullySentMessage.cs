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
