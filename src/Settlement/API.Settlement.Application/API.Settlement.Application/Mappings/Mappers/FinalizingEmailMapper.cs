using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Entities.Emails;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Mappings.Mappers
{
	public class FinalizingEmailMapper : IFinalizingEmailMapper
    {
        private readonly IPDFGenerator _pdfGenerator;
        public FinalizingEmailMapper(IPDFGenerator pdfGenerator)
        {
            _pdfGenerator = pdfGenerator;
        }
        public EmailWithAttachment CreateTransactionSummaryEmailDTO(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO, string subject)
        {
            var pdfBytes = _pdfGenerator.GenerateTransactionSummaryReportPDF(finalizeTransactionResponseDTO);
            var finalizingEmail = new EmailWithAttachment()
            {
                Receiver = finalizeTransactionResponseDTO.UserEmail,
                Subject = subject,
                Body = "Transaction Summary Report - PDF",
                Attachment = pdfBytes,
                AttachmentFileName = "TransactionSummary.pdf",
                AttachmentMimeType = "application/pdf"
            };
            return finalizingEmail;

        }
    }
}
