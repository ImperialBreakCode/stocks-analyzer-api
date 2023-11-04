using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Interfaces
{
	public interface IStocksService
	{
		Task<HttpResponseMessage> GetStockData(string dataType, string companyName);
	}
}
