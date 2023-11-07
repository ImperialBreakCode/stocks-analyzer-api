using API.StockAPI.Infrastructure.Context;
using API.StockAPI.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Infrastructure.Services
{
    public class ContextServices : IContextServices
    {
        private readonly DapperContext _context;
        public ContextServices(DapperContext context)
        {
            _context = context;
        }
    }
}
