using API.Accounts.Domain.Interfaces.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Accounts.Application.Data
{
    public interface IDataSourceFactory
    {
        public IAccountsDbContext Create();
    }
}
