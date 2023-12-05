using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.TransactionInterfaces
{
	public interface ITransactionProcessor
	{
		Task FinalizeTransaction(AvailabilityResponseDTO availabilityResponseDTO);
		Task ProcessFailedTransactions();
	}
}
