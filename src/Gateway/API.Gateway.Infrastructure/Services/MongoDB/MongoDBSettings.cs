using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Infrastructure.Services.MongoDB
{
	public class MongoDBSettings
	{
		public string ConnectionURI { get; set; } = null!;
		public string DatabaseName { get; set; } = null!;
		public string CollectionName { get; set; } = null!;
	}
}
