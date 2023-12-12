namespace API.Settlement.Domain.Interfaces.RabbitMQInterfaces
{
	public interface IRabbitMQConsumer
	{
		string ConsumeMessage(string queue);
	}
}
