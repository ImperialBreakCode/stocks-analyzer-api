using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Analyzer.Domain.DTOs
{
        public class GetUserResponseDTO
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string UserRank { get; set; }
            public string UserEmail { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string? WalletId { get; set; }
        }
    
}
