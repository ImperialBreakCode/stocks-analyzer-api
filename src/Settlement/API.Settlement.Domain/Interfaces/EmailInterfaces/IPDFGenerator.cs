using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;

namespace API.Settlement.Domain.Interfaces.EmailInterfaces
{
	public interface IPDFGenerator
	{
		byte[] GenerateTransactionSummaryReportPDF(FinalizeTransactionResponseDTO data);
	}
}
