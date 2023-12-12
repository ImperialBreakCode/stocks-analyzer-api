using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Helpers.ConstantHelpers
{
	public class MessageConstants : IMessageConstants
	{
		public string TransactionDeclinedMessage => "Transaction declined: Insufficient resources.";
		public string TransactionScheduledMessage => "Transaction scheduled for execution tomorrow at 00:01:00.";
		public string TransactionSuccessMessage => "Transaction completed successfully.";
		public string TransactionConnectionIssueMessage => "Transaction connection issue: Unable to process the transaction at the moment.";
		public string GetMessageBasedOnStatus(Status status)
		{
			switch (status)
			{
				case Status.Declined: return TransactionDeclinedMessage;
				case Status.Success: return TransactionSuccessMessage;
				case Status.Scheduled: return TransactionScheduledMessage;
				default: return string.Empty;

			}
		}
	}
}
