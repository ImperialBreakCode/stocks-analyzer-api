using API.Settlement.Domain.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces
{
	public interface IWalletService
	{
		void UpdateStocksInWallet(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO);
		void CapitalLossCheck();

	}
}
