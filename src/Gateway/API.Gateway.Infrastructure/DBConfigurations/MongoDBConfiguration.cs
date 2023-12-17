namespace API.Gateway.Settings
{
	public class MongoDBConfiguration
	{
		public string ConnectionURI { get; set; } = null!;
		public string DatabaseName { get; set; } = null!;
		public string CollectionName { get; set; } = null!;
	}
}
