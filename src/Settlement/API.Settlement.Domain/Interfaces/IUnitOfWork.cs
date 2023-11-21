﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces
{
	public interface IUnitOfWork
	{
		ISuccessfulTransactionRepository SuccessfulTransactions { get; }
		IFailedTransactionRepository FailedTransactions { get; }
	}
}
