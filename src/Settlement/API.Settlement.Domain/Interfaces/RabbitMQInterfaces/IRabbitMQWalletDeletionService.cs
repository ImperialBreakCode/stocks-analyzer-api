using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.RabbitMQInterfaces
{
	public interface IRabbitMQWalletDeletionService
	{
		void DeleteWallet(string walletId);
	}
}
