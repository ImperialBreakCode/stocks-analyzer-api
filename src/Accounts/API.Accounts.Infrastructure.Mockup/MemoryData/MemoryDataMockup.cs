using API.Accounts.Domain.Interfaces;
using Newtonsoft.Json;

namespace API.Accounts.Infrastructure.Mockup.MemoryData
{
    public class MemoryDataMockup : IMemoryData
    {
        private readonly IDictionary<string, IDictionary<string, object>> _data;
        private readonly IDictionary<string, IDictionary<string, object>> _insertData;
        private readonly IDictionary<string, IDictionary<string, object>> _updateData;
        private readonly IDictionary<string, ICollection<string>> _deletionData;

        public MemoryDataMockup(IDictionary<string, IDictionary<string, object>> data)
        {
            _data = data;
            _insertData = new Dictionary<string, IDictionary<string, object>>();
            _updateData = new Dictionary<string, IDictionary<string, object>>();
            _deletionData = new Dictionary<string, ICollection<string>>();

            MemoryDataHelper.AssignMemoryTables(_insertData);
            MemoryDataHelper.AssignMemoryTables(_updateData);
            MemoryDataHelper.AssignMemoryDeletionTables(_deletionData);
        }

        public void Delete<T>(string id) where T : IEntity
        {
            var table = GetMemoryTable<T>(_data);

            if (table.ContainsKey(id))
            {
                _deletionData[typeof(T).Name].Add(id);
            }
        }

        public T? Get<T>(string id) where T : class, IEntity
        {
            var table = GetMemoryTable<T>(_data);
            T? entity = null;

            if (table.ContainsKey(id))
            {
                entity = CloneObject((T)table[id]);
            }

            return entity;
        }

        public ICollection<T> GetAll<T>() where T : class, IEntity
        {
            var table = GetMemoryTable<T>(_data);
            ICollection<T> entities = new List<T>();

            foreach (var item in table)
            {
                entities.Add(CloneObject((T)item.Value));
            }

            return entities;
        }

        public void Insert<T>(T item) where T : IEntity
        {
            GetMemoryTable<T>(_insertData).Add(item.Id, item);
        }

        public void Update<T>(T item) where T : IEntity
        {
            GetMemoryTable<T>(_updateData).Add(item.Id, item);
        }

        public void SaveChanges()
        {
            object value;
            foreach (var deletionTable in _deletionData)
            {
                foreach (string deletionId in deletionTable.Value)
                {
                    _data[deletionTable.Key].Remove(deletionId);
                }

                deletionTable.Value.Clear();
            }

            foreach (var insertTable in _insertData)
            {
                foreach (var item in insertTable.Value)
                {
                    value = item.Value;
                    _data[insertTable.Key].Add(item.Key, value);
                }

                insertTable.Value.Clear();
            }

            foreach (var updateTable in _updateData)
            {
                foreach (var item in updateTable.Value)
                {
                    if (_data[updateTable.Key].ContainsKey(item.Key))
                    {
                        value = item.Value;
                        _data[updateTable.Key][item.Key] = item.Value;
                    }
                }

                updateTable.Value.Clear();
            }
        }

        private IDictionary<string, object> GetMemoryTable<T>(IDictionary<string, IDictionary<string, object>> dict) where T : IEntity
        {
            return dict[typeof(T).Name];
        }

        private static T CloneObject<T>(T original) where T : class
        {
            string json = JsonConvert.SerializeObject(original);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
