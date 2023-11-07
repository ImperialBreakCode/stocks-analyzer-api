using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces
{
	public interface ITransactionWrapper
	{
		IBuyService BuyService { get; }
		ISellService SellService { get; }
		Task<FinalizeTransactionResponseDTO> CheckAvailability(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO);
	}
}
