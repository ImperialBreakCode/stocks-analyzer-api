using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces
{
	public interface IHttpClient
	{
		Task<string> GetStringAsync(string uri);
		Task<string> PostAsync(string uri, string message);
		Task<string> DeleteAsync(string uri);
	}
}