using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces;
using System.Collections;

namespace API.Accounts.Infrastructure.Mockup.MemoryData
{
    public static class MemoryDataHelper
    {
        public static void AssignMemoryTables(IDictionary<string, IDictionary<string, object>> dict)
        {
            dict.Add(nameof(User), new Dictionary<string, object>());
            dict.Add(nameof(Transaction), new Dictionary<string, object>());
            dict.Add(nameof(Wallet), new Dictionary<string, object>());
            dict.Add(nameof(Stock), new Dictionary<string, object>());
        }

        public static void AssignMemoryDeletionTables(IDictionary<string, ICollection<string>> dict)
        {
            dict.Add(nameof(User), new List<string>());
            dict.Add(nameof(Transaction), new List<string>());
            dict.Add(nameof(Wallet), new List<string>());
            dict.Add(nameof(Stock), new List<string>());
        }
    }
}
