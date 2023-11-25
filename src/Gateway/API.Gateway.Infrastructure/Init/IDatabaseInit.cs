namespace API.Gateway.Infrastructure.Init
{
	public interface IDatabaseInit
	{
		Task PopulateDB();
	}
}