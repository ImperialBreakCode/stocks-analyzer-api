using API.Settlement.Domain.Entities.SQLiteEntities.TransactionDatabaseEntities;

namespace API.Settlement.Domain.Interfaces.HTTPInterfaces
{
	public interface ITransactionResponseHandlerService
	{
		void HandleTransactionResponse(HttpResponseMessage response, IEnumerable<Transaction> transactions);
	}
}
