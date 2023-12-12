using API.Settlement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.HelpersInterfaces
{
	public interface IMessageConstants
	{
		string TransactionDeclinedMessage { get; }
		string TransactionScheduledMessage { get; }
		string TransactionSuccessMessage { get; }
		string TransactionConnectionIssueMessage { get; }
		string GetMessageBasedOnStatus(Status status);
	}
}
