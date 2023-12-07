using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using API.Settlement.Domain.Interfaces.HTMLInterfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Services.EmailServices
{
	public class PDFGenerator : IPDFGenerator
	{
		private readonly IConverter _pdfConverter;
		private readonly IHTMLContentGenerator _htmlContentGenerator;

		public PDFGenerator(IConverter pdfConverter,
							IHTMLContentGenerator htmlContentGenerator)
		{
			_pdfConverter = pdfConverter;
			_htmlContentGenerator = htmlContentGenerator;
		}

		public byte[] GenerateTransactionSummaryReportPDF(FinalizeTransactionResponseDTO data)
		{
			var htmlContent = _htmlContentGenerator.GenerateTransactionSummaryContent(data);

			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings =
				{
					PaperSize = PaperKind.A4,
					Orientation = Orientation.Portrait
				},
				Objects =
				{
					new ObjectSettings
					{
						HtmlContent = htmlContent
					}
				}
			};
			byte[] bytes = _pdfConverter.Convert(doc);
			return bytes;
		}
	}
}
