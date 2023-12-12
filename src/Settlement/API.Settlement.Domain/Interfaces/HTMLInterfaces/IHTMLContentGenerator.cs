using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;

namespace API.Settlement.Domain.Interfaces.HTMLInterfaces
{
	public interface IHTMLContentGenerator
    {
        string GenerateTransactionSummaryContent(FinalizeTransactionResponseDTO data);
    }
}
