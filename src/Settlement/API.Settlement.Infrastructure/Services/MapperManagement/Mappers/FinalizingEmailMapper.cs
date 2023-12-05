using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities.Emails;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using API.Settlement.Domain.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;

namespace API.Settlement.Infrastructure.Services.MapperManagement.Mappers
{
    public class FinalizingEmailMapper : IFinalizingEmailMapper
    {
        private readonly IPDFGenerator _pdfGenerator;
        public FinalizingEmailMapper(IPDFGenerator pdfGenerator)
        {
            _pdfGenerator = pdfGenerator;
        }
        public FinalizingEmail CreateTransactionSummaryEmailDTO(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO, string subject)
        {
            var pdfBytes = _pdfGenerator.GenerateTransactionSummaryReportPDF(finalizeTransactionResponseDTO);
            var finalizingEmail = new FinalizingEmail()
            {
                To = finalizeTransactionResponseDTO.UserEmail,
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
